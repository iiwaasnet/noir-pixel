﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.App.Auth;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Exceptions;
using Api.App.Images;
using Diagnostics;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Artists
{
    public class ArtistsManager : IArtistManager
    {
        private readonly MongoDatabase db;
        private readonly ApplicationUserManager userManager;
        private readonly ILogger logger;
        private readonly IProfileImageManager profileImageManager;

        public ArtistsManager(IAppDbProvider appDbProvider, ApplicationUserManager userManager, IProfileImageManager profileImageManager, ILogger logger)
        {
            db = appDbProvider.GetDatabase();
            this.userManager = userManager;
            this.profileImageManager = profileImageManager;
            this.logger = logger;
        }

        public async Task<ArtistHome> GetUserHome(string userName)
        {
            var users = db.GetCollection<Artist>(Artist.CollectionName);
            var user = users.FindOne(Query.EQ("UserName", userName));

            if (user == null)
            {
                var login = await userManager.FindByNameAsync(userName);
                AssertUserIsRegistered(login);

                user = new Artist
                       {
                           UserName = login.UserName,
                           UserId = login.Id,
                           UserImages = CreateProfileImages(login)
                       };
                users.Insert(user).LogCommandResult(logger);
            }

            return new ArtistHome
                   {
                       UserName = user.UserName,
                       Thumbnail = GetAvatarThumbnail(user)
                   };
        }

        private IEnumerable<ProfileImage> CreateProfileImages(ApplicationUser login)
        {
            if (!string.IsNullOrWhiteSpace(login.ThumbnailImage))
            {
                yield return new ProfileImage
                             {
                                 ImageType = ProfileImageType.Thumbnail,
                                 Url = login.ThumbnailImage,
                                 UserDefined = true
                             };
            }
            else
            {
                yield return new ProfileImage
                             {
                                 ImageType = ProfileImageType.Thumbnail,
                                 Url = profileImageManager.DefaultThumbnailUri().AbsoluteUri,
                                 UserDefined = false
                             };
            }
            yield return new ProfileImage
                         {
                             ImageType = ProfileImageType.Avatar,
                             Url = profileImageManager.DefaultAvatarUri().AbsoluteUri,
                             UserDefined = false
                         };
        }

        private ProfileImage GetAvatarThumbnail(Artist artist)
        {
            var userImages = (artist.UserImages != null)
                                 ? artist.UserImages.ToList()
                                 : new List<ProfileImage>();

            var thumbnail = userImages.FirstOrDefault(i => i.ImageType == ProfileImageType.Thumbnail);
            if (thumbnail == null)
            {
                thumbnail = new ProfileImage
                            {
                                ImageType = ProfileImageType.Thumbnail,
                                Url = profileImageManager.DefaultThumbnailUri().AbsoluteUri,
                                UserDefined = false
                            };
                userImages.Add(thumbnail);
                artist.UserImages = userImages;
                db.GetCollection<Artist>(Artist.CollectionName).Save(artist).LogCommandResult(logger);
            }

            return thumbnail;
        }

        private void AssertUserIsRegistered(ApplicationUser login)
        {
            if (login == null)
            {
                throw new NotFoundException();
            }
        }
    }
}
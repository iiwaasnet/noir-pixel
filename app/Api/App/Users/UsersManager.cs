using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Antlr.Runtime.Misc;
using Api.App.Auth;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Exceptions;
using Api.App.Images;
using Api.App.Users.Entities;
using Diagnostics;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Users
{
    public class UsersManager : IUsersManager
    {
        private readonly MongoDatabase db;
        private readonly ApplicationUserManager accountsManager;
        private readonly ILogger logger;
        private readonly IProfileImageManager profileImageManager;

        public UsersManager(IAppDbProvider appDbProvider,
                            ApplicationUserManager accountsManager,
                            IProfileImageManager profileImageManager,
                            ILogger logger)
        {
            db = appDbProvider.GetDatabase();
            this.accountsManager = accountsManager;
            this.profileImageManager = profileImageManager;
            this.logger = logger;
        }

        public async Task<UserHome> GetUserHome(string userName)
        {
            var users = db.GetCollection<User>(User.CollectionName);
            var user = users.FindOne(Query.EQ("UserName", userName));

            if (user == null)
            {
                var login = await accountsManager.FindByNameAsync(userName);
                AssertUserIsRegistered(login);

                user = new User
                       {
                           UserName = login.UserName,
                           UserId = login.Id,
                           UserImages = CreateProfileImages(login)
                       };
                users.Insert(user).LogCommandResult(logger);
            }

            var thumbnail = GetAvatarThumbnail(user);
            return new UserHome
                   {
                       User = new UserReference
                              {
                                  UserName = user.UserName,
                                  FullName = user.FullName
                              },
                       Thumbnail = (thumbnail != null)
                                       ? new ProfileImage
                                         {
                                             Url = thumbnail.Url,
                                             UserDefined = thumbnail.UserDefined
                                         }
                                       : null
                   };
        }

        public UserProfile GetUserProfile(string userName, bool includePrivateData)
        {
            var users = db.GetCollection<User>(User.CollectionName);
            var user = users.FindOne(Query.EQ("UserName", userName));

            var avatar = GetAvatar(user);
            var thumbnail = GetAvatarThumbnail(user);

            var profile = new UserProfile
                          {
                              PublicInfo = new UserPublicInfo
                                           {
                                               UserName = user.UserName,
                                               FullName = user.FullName,
                                               DateRegistered = user.DateRegistered,
                                               LivesIn = user.LivesIn,
                                               Avatar = new ProfileImage
                                                        {
                                                            Url = avatar.Url,
                                                            UserDefined = avatar.UserDefined
                                                        },
                                               Thumbnail = (thumbnail != null)
                                                               ? new ProfileImage
                                                                 {
                                                                     Url = thumbnail.Url,
                                                                     UserDefined = thumbnail.UserDefined
                                                                 }
                                                               : null,
                                               AboutMe = user.AboutMe
                                           },
                              PrivateInfo = (includePrivateData)
                                                ? new UserPrivateInfo {Email = user.Email}
                                                : null
                          };

            return profile;
        }

        private IEnumerable<Entities.ProfileImage> CreateProfileImages(ApplicationUser login)
        {
            if (!string.IsNullOrWhiteSpace(login.ThumbnailImage))
            {
                yield return new Entities.ProfileImage
                             {
                                 ImageType = ProfileImageType.Thumbnail,
                                 Url = login.ThumbnailImage,
                                 UserDefined = true
                             };
            }

            yield return CreateDefaultAvatar();
        }

        private Entities.ProfileImage CreateDefaultAvatar()
        {
            return new Entities.ProfileImage
                   {
                       ImageType = ProfileImageType.Avatar,
                       Url = profileImageManager.DefaultAvatarUri().AbsoluteUri,
                       UserDefined = false
                   };
        }

        private Entities.ProfileImage GetAvatarThumbnail(User user)
        {
            return GetProfileImage(user, ProfileImageType.Thumbnail, null);
        }

        private Entities.ProfileImage GetAvatar(User user)
        {
            return GetProfileImage(user, ProfileImageType.Avatar, CreateDefaultAvatar);
        }

        private Entities.ProfileImage GetProfileImage(User user, ProfileImageType immageType, Func<Entities.ProfileImage> @default)
        {
            var userImages = (user.UserImages != null)
                                 ? user.UserImages.ToList()
                                 : new List<Entities.ProfileImage>();

            var profileImage = userImages.FirstOrDefault(i => i.ImageType == immageType);
            if (profileImage == null && @default != null)
            {
                profileImage = @default();
                userImages.Add(profileImage);
                user.UserImages = userImages;
                db.GetCollection<User>(User.CollectionName).Save(user).LogCommandResult(logger);
            }

            return profileImage;
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
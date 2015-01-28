using System.Collections.Generic;
using System.Threading.Tasks;
using Api.App.Auth;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Exceptions;
using Api.App.Images;
using Api.App.Profiles.Entities;
using Diagnostics;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Profiles
{
    public class ProfilesManager : IProfilesManager
    {
        private readonly MongoDatabase db;
        private readonly ApplicationUserManager accountsManager;
        private readonly IProfileImageManager profileImageManager;
        private readonly ILogger logger;

        public ProfilesManager(IAppDbProvider appDbProvider,
                               ApplicationUserManager accountsManager,
                               IProfileImageManager profileImageManager,
                               ILogger logger)
        {
            db = appDbProvider.GetDatabase();
            this.accountsManager = accountsManager;
            this.profileImageManager = profileImageManager;
            this.logger = logger;
        }

        public IEnumerable<Country> GetCountries()
        {
            var countries = db.GetCollection<Country>(Country.CollectionName);

            return countries.FindAll();
        }

        public async Task<UserProfile> GetUserProfile(string userName, bool includePrivateData)
        {
            var profile = await EnsureUserProfileExists(userName);

            var userProfile = new UserProfile
                              {
                                  User = new UserReference
                                         {
                                             UserName = profile.UserName,
                                             FullName = profile.FullName
                                         },
                                  PublicInfo = new UserPublicInfo
                                               {
                                                   DateRegistered = profile.DateRegistered,
                                                   LivesIn = (profile.LivesIn != null)
                                                                 ? new Geo
                                                                   {
                                                                       CountryCode = profile.LivesIn.CountryCode,
                                                                       Country = profile.LivesIn.Country,
                                                                       City = profile.LivesIn.City
                                                                   }
                                                                 : null,
                                                   Avatar = GetFullViewUrl(profile),
                                                   Thumbnail = GetThumbnailUrl(profile),
                                                   AboutMe = profile.AboutMe
                                               },
                                  PrivateInfo = (includePrivateData)
                                                    ? new UserPrivateInfo {Email = profile.Email}
                                                    : null
                              };

            return userProfile;
        }

        private string GetThumbnailUrl(Profile profile)
        {
            return (profile.UserImage != null && profile.UserImage.Thumbnail != null)
                       ? profile.UserImage.Thumbnail.Uri
                       : null;
        }

        private string GetFullViewUrl(Profile profile)
        {
            return (profile.UserImage != null && profile.UserImage.FullView != null)
                       ? profile.UserImage.FullView.Uri
                       : null;
        }

        private async Task<Profile> EnsureUserProfileExists(string userName)
        {
            var users = db.GetCollection<Profile>(Profile.CollectionName);
            var user = users.FindOne(Query<Profile>.EQ(p => p.UserName, userName));

            if (user == null)
            {
                var login = await accountsManager.FindByNameAsync(userName);
                AssertUserIsRegistered(login);

                user = new Profile
                       {
                           UserName = login.UserName,
                           UserId = login.Id
                       };
                users.Insert(user).LogCommandResult(logger);
                CreateProfileThumbnail(login);
            }
            return user;
        }

        private void CreateProfileThumbnail(ApplicationUser login)
        {
            if (!string.IsNullOrWhiteSpace(login.ThumbnailImage))
            {
                profileImageManager.SaveThumbnailLink(login.UserName, login.ThumbnailImage);
            }
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
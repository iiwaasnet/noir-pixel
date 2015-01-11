using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.App.Auth;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Exceptions;
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
        private readonly ILogger logger;

        public ProfilesManager(IAppDbProvider appDbProvider,
                               ApplicationUserManager accountsManager,
                               ILogger logger)
        {
            db = appDbProvider.GetDatabase();
            this.accountsManager = accountsManager;
            this.logger = logger;
        }

        public async Task<UserProfile> GetUserProfile(string userName, bool includePrivateData)
        {
            var user = await EnsureUserProfileExists(userName);

            var avatar = GetProfileImage(user, ProfileImageType.Avatar);
            var thumbnail = GetProfileImage(user, ProfileImageType.Thumbnail);

            var profile = new UserProfile
                          {
                              User = new UserReference
                                     {
                                         UserName = user.UserName,
                                         FullName = user.FullName
                                     },
                              PublicInfo = new UserPublicInfo
                                           {
                                               DateRegistered = user.DateRegistered,
                                               LivesIn = (user.LivesIn != null)
                                                             ? new Geo
                                                               {
                                                                   CountryCode = user.LivesIn.CountryCode,
                                                                   Country = user.LivesIn.Country,
                                                                   City = user.LivesIn.City
                                                               }
                                                             : null,
                                               Avatar = (avatar != null) ? avatar.Url : null,
                                               Thumbnail = (thumbnail != null) ? thumbnail.Url : null,
                                               AboutMe = user.AboutMe
                                           },
                              PrivateInfo = (includePrivateData)
                                                ? new UserPrivateInfo {Email = user.Email}
                                                : null
                          };

            return profile;
        }

        private async Task<Profile> EnsureUserProfileExists(string userName)
        {
            var users = db.GetCollection<Profile>(Profile.CollectionName);
            var user = users.FindOne(Query.EQ("UserName", userName));

            if (user == null)
            {
                var login = await accountsManager.FindByNameAsync(userName);
                AssertUserIsRegistered(login);

                user = new Profile
                       {
                           UserName = login.UserName,
                           UserId = login.Id,
                           UserImages = CreateProfileThumbnail(login)
                       };
                users.Insert(user).LogCommandResult(logger);
            }
            return user;
        }

        private IEnumerable<ProfileImage> CreateProfileThumbnail(ApplicationUser login)
        {
            if (!string.IsNullOrWhiteSpace(login.ThumbnailImage))
            {
                yield return new ProfileImage
                             {
                                 ImageType = ProfileImageType.Thumbnail,
                                 Url = login.ThumbnailImage
                             };
            }
        }

        private ProfileImage GetProfileImage(Profile profile, ProfileImageType immageType)
        {
            return (profile.UserImages ?? Enumerable.Empty<ProfileImage>())
                .FirstOrDefault(i => i.ImageType == immageType);
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
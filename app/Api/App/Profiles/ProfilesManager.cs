using System.Threading.Tasks;
using Api.App.Auth;
using Api.App.Db;
using Api.App.Geo;
using Api.App.Images;
using Api.App.Profiles.Entities;
using Diagnostics;
using MongoDB.Driver;

namespace Api.App.Profiles
{
    public partial class ProfilesManager : IProfilesManager
    {
        private readonly IMongoDatabase db;
        private readonly ApplicationUserManager accountsManager;
        private readonly IProfileImageManager profileImageManager;
        private readonly ILogger logger;
        private readonly IGeoManager geoManager;

        public ProfilesManager(IAppDbProvider appDbProvider,
                               ApplicationUserManager accountsManager,
                               IProfileImageManager profileImageManager,
                               IGeoManager geoManager,
                               ILogger logger)
        {
            db = appDbProvider.GetDatabase();
            this.accountsManager = accountsManager;
            this.profileImageManager = profileImageManager;
            this.geoManager = geoManager;
            this.logger = logger;
        }

        public async Task<UserProfile> GetUserProfile(string userName, bool includePrivateData)
        {
            var profile = await EnsureUserProfileExists(userName);

            var userProfile = new UserProfile
                              {
                                  PublicInfo = new UserPublicInfo
                                               {
                                                   User = new UserReference
                                                          {
                                                              UserName = profile.UserName,
                                                              FullName = profile.FullName
                                                          },
                                                   DateRegistered = profile.DateRegistered,
                                                   LivesIn = (profile.LivesIn != null)
                                                                 ? new Geo
                                                                   {
                                                                       CountryCode = profile.LivesIn.CountryCode,
                                                                       Country = profile.LivesIn.Country,
                                                                       City = profile.LivesIn.City
                                                                   }
                                                                 : null,
                                                   ProfileImage = GetFullViewUrl(profile),
                                                   ProfileImageThumbnail = GetThumbnailUrl(profile),
                                                   AboutMe = profile.AboutMe
                                               },
                                  PrivateInfo = (includePrivateData)
                                                    ? new UserPrivateInfo {Email = profile.Email}
                                                    : null
                              };

            return userProfile;
        }

        public async Task UpdatePublicInfo(string userName, ProfilePublicInfo info)
        {
            var collection = db.GetCollection<Profile>(Profile.CollectionName);

            var geo = await MapCountryName(info);

            var builder = new UpdateDefinitionBuilder<Profile>();
            var update = builder.Combine(new[]
                                         {
                                             builder.Set(p => p.FullName, info.UserFullName),
                                             builder.Set(p => p.LivesIn, geo)
                                         });

            await collection.FindOneAndUpdateAsync(p => p.UserName == userName, update);
        }

        public async Task UpdatePrivateInfo(string userName, ProfilePrivateInfo info)
        {
            var collection = db.GetCollection<Profile>(Profile.CollectionName);

            var builder = new UpdateDefinitionBuilder<Profile>();
            var update = builder.Set(p => p.Email, info.Email);

            await collection.FindOneAndUpdateAsync(p => p.UserName == userName, update);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.App.Auth;
using Api.App.Db;
using Api.App.Images;
using Api.App.Profiles.Entities;
using Diagnostics;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Profiles
{
    public partial class ProfilesManager : IProfilesManager
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

        public void UpdatePublicInfo(string userName, ProfilePublicInfo publicInfo)
        {
            var collection = db.GetCollection<Profile>(Profile.CollectionName);
            var update = new FindAndModifyArgs
                         {
                             Query = Query<Profile>.EQ(p => p.UserName, userName),
                             Update = Update<Profile>.Combine(Update<Profile>.Set(p => p.FullName, publicInfo.UserFullName),
                                                              Update<Profile>.Set(p => p.LivesIn, new Entities.Geo()))
                         };
            collection.FindAndModify(update);
        }
    }
}
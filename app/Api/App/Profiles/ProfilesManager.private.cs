using System.Threading.Tasks;
using Api.App.Auth;
using Api.App.Db.Extensions;
using Api.App.Exceptions;
using Api.App.Profiles.Entities;
using AspNet.Identity.MongoDB;
using MongoDB.Driver;

namespace Api.App.Profiles
{
    public partial class ProfilesManager
    {
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
            var user = await users.Find(p => p.UserName == userName).FirstOrDefaultAsync();

            try
            {
                if (user == null)
                {
                    var login = await accountsManager.FindByNameAsync(userName);
                    AssertUserIsRegistered(login);

                    user = new Profile
                           {
                               UserName = login.UserName,
                               UserId = login.Id
                           };
                    await users.InsertOneAsync(user);
                    CreateProfileThumbnail((ApplicationUser) login);
                    user = await db.GetProfile(userName);
                }
            }
            catch (MongoDuplicateKeyException)
            {
            }

            return user ?? await users.Find(p => p.UserName == userName).FirstOrDefaultAsync();
        }

        private void CreateProfileThumbnail(ApplicationUser login)
        {
            if (!string.IsNullOrWhiteSpace(login.ThumbnailImage))
            {
                profileImageManager.SaveThumbnailLink(login.UserName, login.ThumbnailImage);
            }
        }

        private void AssertUserIsRegistered(IdentityUser login)
        {
            if (login == null)
            {
                throw new NotFoundException();
            }
        }

        private async Task<Entities.Geo> MapCountryName(ProfilePublicInfo publicInfo)
        {
            var country = await geoManager.GetCountry(publicInfo.LivesIn.CountryCode);

            var geo = new Entities.Geo {City = publicInfo.LivesIn.City};
            if (country != null)
            {
                geo.CountryCode = country.Code;
                geo.Country = country.Name;
            }

            return geo;
        }
    }
}
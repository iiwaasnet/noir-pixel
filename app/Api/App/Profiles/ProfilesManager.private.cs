using System.Threading.Tasks;
using Api.App.Auth;
using Api.App.Db.Extensions;
using Api.App.Exceptions;
using Api.App.Profiles.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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
            var user = users.FindOne(Query<Profile>.EQ(p => p.UserName, userName));

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
                    users.Insert(user).LogCommandResult(logger);
                    CreateProfileThumbnail(login);
                    user = db.GetProfile(userName);
                }
            }
            catch (MongoDuplicateKeyException)
            {
                user = users.FindOne(Query<Profile>.EQ(p => p.UserName, userName));
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

        private Entities.Geo MapCountryName(ProfilePublicInfo publicInfo)
        {
            var country = geoManager.GetCountry(publicInfo.LivesIn.CountryCode);

            var geo = new Entities.Geo { City = publicInfo.LivesIn.City };
            if (country != null)
            {
                geo.CountryCode = country.Code;
                geo.Country = country.Name;
            }

            return geo;
        }
    }
}
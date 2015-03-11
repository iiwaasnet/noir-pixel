using System.Threading.Tasks;
using Api.App.Exceptions;
using Api.App.Profiles.Entities;
using MongoDB.Driver;

namespace Api.App.Db.Extensions
{
    public static class Profiles
    {
        public async static Task<Profile> GetProfile(this IMongoDatabase db, string userName)
        {
            var profiles = db.GetCollection<Profile>(Profile.CollectionName);
            var profile = await profiles.Find(p => p.UserName == userName).SingleOrDefaultAsync();

            if (profile == null)
            {
                throw new NotFoundException(userName);
            }

            return profile;
        }
    }
}
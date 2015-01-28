using Api.App.Exceptions;
using Api.App.Profiles.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Db.Extensions
{
    public static class Profiles
    {
        public static Profile GetProfile(this MongoDatabase db, string userName)
        {
            var profiles = db.GetCollection<Profile>(Profile.CollectionName);
            var profile = profiles.FindOne(Query<Profile>.EQ(p => p.UserName, userName));

            if (profile == null)
            {
                throw new NotFoundException(userName);
            }

            return profile;
        }
    }
}
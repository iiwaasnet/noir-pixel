using Api.App.Db.Config;
using MongoDB.Driver;

namespace Api.App.Db
{
    public class DbProvider : IDbProvider
    {
        private readonly MongoDatabase database;

        public DbProvider(DbConfiguration config)
        {
            var client = new MongoClient(config.Server);
            database = client.GetServer().GetDatabase(config.Database);
        }

        public MongoDatabase GetDatabase()
        {
            return database;
        }
    }
}
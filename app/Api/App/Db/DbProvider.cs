using Api.App.Db.Config;
using MongoDB.Driver;

namespace Api.App.Db
{
    public class DbProvider : IDbProvider
    {
        private readonly IMongoDatabase database;

        public DbProvider(DbConfiguration config)
        {
            var client = new MongoClient(config.Server);
            database = client.GetDatabase(config.Database);
        }

        public IMongoDatabase GetDatabase()
        {
            return database;
        }
    }
}
using MongoDB.Driver;

namespace Api.Core.Db
{
    public interface IDbProvider
    {
        MongoDatabase GetDatabase();
    }
}
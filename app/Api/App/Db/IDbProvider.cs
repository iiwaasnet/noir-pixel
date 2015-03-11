using MongoDB.Driver;

namespace Api.App.Db
{
    public interface IDbProvider
    {
        IMongoDatabase GetDatabase();
    }
}
using Api.App.Db;
using MongoDB.Driver;

namespace Api.App.Users
{
    public class UsersManager : IUserManager
    {
        private readonly MongoDatabase db;

        public UsersManager(IDbProvider dbProvider)
        {
            db = dbProvider.GetDatabase();
        }

        public UserHome GetUserHome(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}
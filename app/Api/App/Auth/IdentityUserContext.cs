using System;
using Api.App.Db;
using AspNet.Identity.MongoDB;
using MongoDB.Driver;

namespace Api.App.Auth
{
    public class IdentityUserContext : UsersContext<ApplicationUser>, IDisposable
    {
        public IdentityUserContext(IMongoCollection<ApplicationUser> users)
            : base(users)
        {
        }

        public static IdentityUserContext Create(IIdentityDbProvider dbProvider)
        {
            var database = dbProvider.GetDatabase();
            var users = database.GetCollection<ApplicationUser>("users");
            users.EnsureUniqueIndexOnEmail();
            users.EnsureUniqueIndexOnUserName();

            return new IdentityUserContext(users);
        }

        public void Dispose()
        {
        }
    }
}
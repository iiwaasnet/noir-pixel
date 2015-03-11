using System;
using Api.App.Db;
using AspNet.Identity.MongoDB;
using MongoDB.Driver;

namespace Api.App.Auth
{
    public class IdentityRoleContext : RolesContext<IdentityRole>, IDisposable
    {
        public IdentityRoleContext(IMongoCollection<IdentityRole> roles) 
            : base(roles)
        {
        }

        public static IdentityRoleContext Create(IIdentityDbProvider dbProvider)
        {
            var database = dbProvider.GetDatabase();
            var roles = database.GetCollection<IdentityRole>("roles");
            roles.EnsureUniqueIndexOnRoleName();

            return new IdentityRoleContext(roles);
        }

        public void Dispose()
        {
        }
    }
}
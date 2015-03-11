using System;
using Api.App.Db;
using AspNet.Identity.MongoDB;
using MongoDB.Driver;

namespace Api.App.Auth
{
    //public class ApplicationIdentityContext : IdentityContext, IDisposable
    //{
    //    public ApplicationIdentityContext(IMongoCollection<IdentityUser> users, IMongoCollection<IdentityRole> roles)
    //        : base(users, roles)
    //    {
    //    }

    //    public static ApplicationIdentityContext Create(IIdentityDbProvider dbProvider)
    //    {
    //        var database = dbProvider.GetDatabase();
    //        var users = database.GetCollection<IdentityUser>("users");
    //        var roles = database.GetCollection<IdentityRole>("roles");

    //        return new ApplicationIdentityContext(users, roles);
    //    }

    //    public void Dispose()
    //    {
    //    }
    //}
}
﻿using System;
using Api.App.Db;
using AspNet.Identity.MongoDB;
using JsonConfigurationProvider;
using MongoDB.Driver;

namespace Api.App.Auth
{
    public class ApplicationIdentityContext : IdentityContext, IDisposable
    {
        public ApplicationIdentityContext(MongoCollection users, MongoCollection roles)
            : base(users, roles)
        {
        }

        public static ApplicationIdentityContext Create(IConfigProvider configProvider)
        {
            var config = configProvider.GetConfiguration<DbServerConfiguration>();

            var client = new MongoClient(config.Server);
            var database = client.GetServer().GetDatabase(config.Identity.Database);
            var users = database.GetCollection<IdentityUser>("users");
            var roles = database.GetCollection<IdentityRole>("roles");

            return new ApplicationIdentityContext(users, roles);
        }

        public void Dispose()
        {
        }
    }
}
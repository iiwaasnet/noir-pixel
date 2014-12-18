using Api.App.Db.Config;
using Autofac;

namespace Api.App.Db
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new DbProvider(c.ResolveNamed<DbConfiguration>(Databases.Identity))).Named<IDbProvider>(Databases.Identity);
            builder.Register(c => new DbProvider(c.ResolveNamed<DbConfiguration>(Databases.Np))).Named<IDbProvider>(Databases.Np);
        }
    }

    public class Databases
    {
        public const string Identity = "Identity";
        public const string Np = "Np";
    }
}
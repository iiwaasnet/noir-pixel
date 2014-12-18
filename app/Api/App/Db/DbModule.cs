using Api.App.Db.Config;
using Autofac;
using JsonConfigurationProvider;

namespace Api.App.Db
{
    public class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => GetDbConfigurations(c).Application)
                   .Named<DbConfiguration>(Databases.Application)
                   .SingleInstance();
            builder.Register(c => GetDbConfigurations(c).Identity)
                   .Named<DbConfiguration>(Databases.Identity)
                   .SingleInstance();

            builder.Register(c => new DbProvider(c.ResolveNamed<DbConfiguration>(Databases.Identity)))
                   .As<IIdentityDbProvider>()
                   .SingleInstance();
            builder.Register(c => new DbProvider(c.ResolveNamed<DbConfiguration>(Databases.Application)))
                   .As<IAppDbProvider>()
                   .SingleInstance();
        }

        private static DbSourcesConfiguration GetDbConfigurations(IComponentContext c)
        {
            return c.Resolve<IConfigProvider>().GetConfiguration<DbSourcesConfiguration>();
        }
    }

    public class Databases
    {
        public const string Identity = "Identity";
        public const string Application = "Application";
    }
}
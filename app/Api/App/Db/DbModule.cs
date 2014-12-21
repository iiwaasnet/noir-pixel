using Api.App.Db.Config;
using Autofac;
using JsonConfigurationProvider;

namespace Api.App.Db
{
    public class DbModule : Module
    {
        private const string Identity = "Identity";
        private const string Application = "Application";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => GetDbConfigurations(c).Application)
                   .Named<DbConfiguration>(Application)
                   .SingleInstance();
            builder.Register(c => GetDbConfigurations(c).Identity)
                   .Named<DbConfiguration>(Identity)
                   .SingleInstance();

            builder.Register(c => new IdentityDbProvider(c.ResolveNamed<DbConfiguration>(Identity)))
                   .As<IIdentityDbProvider>()
                   .SingleInstance();
            builder.Register(c => new AppDbProvider(c.ResolveNamed<DbConfiguration>(Application)))
                   .As<IAppDbProvider>()
                   .SingleInstance();
        }

        private static DbSourcesConfiguration GetDbConfigurations(IComponentContext c)
        {
            return c.Resolve<IConfigProvider>().GetConfiguration<DbSourcesConfiguration>();
        }
    }
}
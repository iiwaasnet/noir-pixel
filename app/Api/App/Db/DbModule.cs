using Api.App.Db.Config;
using Autofac;
using TypedConfigProvider;

namespace Api.App.Db
{
    public class DbModule : Module
    {
        private const string Identity = "Identity";
        private const string Application = "Application";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => GetDbConfigurations(c).Application)
                   .Named<ConnectionConfiguration>(Application)
                   .SingleInstance();
            builder.Register(c => GetDbConfigurations(c).Identity)
                   .Named<ConnectionConfiguration>(Identity)
                   .SingleInstance();

            builder.Register(c => new IdentityDbProvider(c.ResolveNamed<ConnectionConfiguration>(Identity)))
                   .As<IIdentityDbProvider>()
                   .SingleInstance();
            builder.Register(c => new AppDbProvider(c.ResolveNamed<ConnectionConfiguration>(Application)))
                   .As<IAppDbProvider>()
                   .SingleInstance();
        }

        private static DbConfiguration GetDbConfigurations(IComponentContext c)
        {
            return c.Resolve<IConfigProvider>().GetConfiguration<DbConfiguration>();
        }
    }
}
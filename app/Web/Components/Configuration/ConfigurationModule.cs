using Autofac;
using TypedConfigProvider;

namespace Web.Components.Configuration
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigProvider>()
                   .As<IConfigProvider>()
                   .SingleInstance();

            builder.RegisterType<AppConfigTargetProvider>()
                   .As<IConfigTargetProvider>()
                   .SingleInstance();
        }
    }
}
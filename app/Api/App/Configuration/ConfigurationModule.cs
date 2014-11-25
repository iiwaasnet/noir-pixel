using Autofac;
using Diagnostics;
using JsonConfigurationProvider;

namespace Api.App.Configuration
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new JsonConfigProvider(c.Resolve<IConfigTargetProvider>(),
                                                         "config",
                                                         c.Resolve<ILogger>()))
                   .As<IConfigProvider>()
                   .SingleInstance();
            builder.Register(c => new JsonConfigTargetProvider("config/Environment.config.json"))
                   .As<IConfigTargetProvider>()
                   .SingleInstance();
        }
    }
}
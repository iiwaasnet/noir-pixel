using Autofac;
using JsonConfigurationProvider;

namespace Web.Components.Configuration
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonConfigProvider>().As<IConfigProvider>().SingleInstance();
            builder.Register(c => new JsonConfigTargetProvider("config/Environment.config.json")).As<IConfigTargetProvider>().SingleInstance();
        }
    }
}
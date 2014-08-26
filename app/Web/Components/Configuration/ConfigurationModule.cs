using Autofac;
using JsonConfigurationProvider;

namespace Web.Components.Configuration
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JsonConfigProvider>().As<IJsonConfigProvider>().SingleInstance();
            builder.Register(c => new ConfigTargetProvider("build-target.config")).As<IConfigTargetProvider>().SingleInstance();
        }
    }
}
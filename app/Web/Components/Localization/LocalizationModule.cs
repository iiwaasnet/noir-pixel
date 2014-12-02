using Autofac;
using Resources;
using Resources.Client;

namespace Web.Components.Localization
{
    public class LocalizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ClientStringsProvider>().As<IClientStringsProvider>().SingleInstance();
            builder.RegisterType<LocalizedStrings>().As<ILocalizedStrings>().SingleInstance();
        }
    }
}
using Autofac;
using Resources;

namespace Web.Components.Localization
{
    public class LocalizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StringsProvider>().As<IStringsProvider>().SingleInstance();
            builder.RegisterType<LocalizedStrings>().As<ILocalizedStrings>().SingleInstance();
        }
    }
}
using Autofac;
using Resources;

namespace Api.App.Resources
{
    public class StringsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StringsProvider>().As<IStringsProvider>().SingleInstance();
        }
    }
}
using Autofac;
using FluentValidation.Resources;
using Resources.Api;

namespace Api.App.Resources
{
    public class StringsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiStringsProvider>().As<IApiStringsProvider>().SingleInstance();            
        }
    }
}
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Diagnostics;
using Module = Autofac.Module;

namespace Api.Logging
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
        }
    }
}
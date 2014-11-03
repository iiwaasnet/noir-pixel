using System.Reflection;
using System.Web.Http.ExceptionHandling;
using Autofac;
using Autofac.Integration.WebApi;
using Diagnostics;
using Module = Autofac.Module;

namespace Api
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<Logger>()
                   .As<ILogger>()
                   .SingleInstance();

            builder.RegisterType<CustomExceptionLogger>()
                   .As<IExceptionLogger>()
                   .SingleInstance();
        }
    }
}
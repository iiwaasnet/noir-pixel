using System.Reflection;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using Api.App.Errors;
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
            builder.RegisterType<ApiExceptionLogger>()
                   .As<IExceptionLogger>()
                   .SingleInstance();
            builder.RegisterType<ApiExceptionHandler>()
                   .As<IExceptionHandler>()
                   .SingleInstance();
            builder.RegisterType<HttpExceptionLoggingFilter>()
                   .As<IActionFilter>()
                   .SingleInstance();
        }
    }
}
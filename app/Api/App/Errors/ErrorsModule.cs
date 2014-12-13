using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using Autofac;

namespace Api.App.Errors
{
    public class ErrorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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
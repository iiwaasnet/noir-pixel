using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using Api.App.Validation;
using Autofac;
using Ext.FluentValidation.WebApi;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            // TODO: Uncomment if every HttpError needs to be logged
            //config.Filters.Add(DependencyInjection.GetContainer().Resolve<IActionFilter>());
            config.Services.Replace(typeof (IExceptionLogger), DependencyInjection.GetContainer().Resolve<IExceptionLogger>());
            config.Services.Replace(typeof (IExceptionHandler), DependencyInjection.GetContainer().Resolve<IExceptionHandler>());

            var formatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            FluentValidationModelValidatorProvider.Configure(config, DependencyInjection.GetContainer().Resolve<Action<FluentValidationModelValidatorProvider>>());
            config.Services.Replace(typeof (IBodyModelValidator), new PrefixlessModelValidator(config.Services.GetBodyModelValidator()));

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                                       name: "DefaultApi",
                                       routeTemplate: "{controller}/{id}",
                                       defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Validation;
using Api.Validation;
using Autofac;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Services.Add(typeof(IExceptionLogger), DependencyInjection.GetContainer().Resolve<IExceptionLogger>());

            var formatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Services.Replace(typeof(IBodyModelValidator), new PrefixlessModelValidator(config.Services.GetBodyModelValidator()));

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                                       name: "DefaultApi",
                                       routeTemplate: "{controller}/{id}",
                                       defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}
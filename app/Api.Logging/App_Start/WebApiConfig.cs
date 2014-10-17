﻿using System.Web.Http;
using System.Web.Http.Cors;

namespace Api.Logging
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //TODO: Enable authentication if needed
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.EnableCors(new EnableCorsAttribute("http://noir-pixel.com", "*", "GET,POST"));
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                                       name: "DefaultApi",
                                       routeTemplate: "{controller}/{id}",
                                       defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}
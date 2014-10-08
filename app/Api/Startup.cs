using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Api;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAutofacMiddleware(DependencyInjection.GetContainer());

            var config = new HttpConfiguration
                         {
                             DependencyResolver = new AutofacWebApiDependencyResolver(DependencyInjection.GetContainer())
                         };
            // TODO: Do the same in Web project instead of custom Json result
            var formatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            app.UseWebApi(config);
            ConfigureAuth(app);
        }
    }
}
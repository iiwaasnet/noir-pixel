using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Api.Logging;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace Api.Logging
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAutofacMiddleware(DependencyInjection.GetContainer());

            //ConfigureAuth(app);

            var config = new HttpConfiguration
                         {
                             DependencyResolver = new AutofacWebApiDependencyResolver(DependencyInjection.GetContainer())
                         };
            
            app.UseWebApi(config);
            app.UseCors(new CorsOptions());
        }
    }
}
using System.Web.Http;
using Api;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
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

            ConfigureAuth(app);

            app.UseWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
        }
    }
}
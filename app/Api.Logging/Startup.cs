using Api.Logging;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof (Startup))]

namespace Api.Logging
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.UseAutofacMiddleware(DependencyInjection.GetContainer());
            app.UseCors(new CorsOptions());
        }
    }
}
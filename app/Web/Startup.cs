using Microsoft.Owin;
using Owin;
using Web;
using Web.Components;

[assembly: OwinStartup(typeof (Startup))]

namespace Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseAutofacMiddleware(DependencyInjection.GetContainer());
            //app.UseDeviceDetectionMiddleware();
        }
    }
}
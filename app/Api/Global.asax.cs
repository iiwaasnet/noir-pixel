using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.WebApi;

namespace Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(DependencyInjection.GetContainer()));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(DependencyInjection.GetContainer());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
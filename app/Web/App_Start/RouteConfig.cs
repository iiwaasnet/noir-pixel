using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("StringResources",
                            "{controller}/{action}/{lang}",
                            new
                            {
                                controller = "Strings",
                                lang = UrlParameter.Optional
                            });
            routes.MapRoute("RedirectToHome",
                            "{*url}",
                            new {controller = "Home", action = "Index"});
        }
    }
}
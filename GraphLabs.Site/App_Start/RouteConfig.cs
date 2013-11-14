using System.Web.Mvc;
using System.Web.Routing;

namespace GraphLabs.Site.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "TaskVariant",
                url: "TaskVariant/{taskId}/{action}/{variantId}",
                defaults:
                    new {controller = "TaskVariant", action = "Index", taskId = -1, variantId = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}
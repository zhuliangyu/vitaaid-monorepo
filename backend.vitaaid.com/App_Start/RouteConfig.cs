using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace backend.vitaaid.com
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute(
                name: "Default", // Route name
                url: "{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Member", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}
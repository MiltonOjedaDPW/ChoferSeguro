using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LoyaltyMovil
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{chofer}/{rntt}",
                defaults: new { controller = "Home", action = "Index", rntt = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ChoferRNTT",
                url: "GetRegistroPorChofer/{rntt}",
                defaults: new { controller = "Home", action = "GetRegistroPorChofer", rntt = UrlParameter.Optional }
            );


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReportingNew
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{userGuid}/{repid}",
                defaults: new { controller = "Home", 
                    action = "Index",
                    userGuid = UrlParameter.Optional, 
                    repid=UrlParameter.Optional  }
            );
        }
    }
}

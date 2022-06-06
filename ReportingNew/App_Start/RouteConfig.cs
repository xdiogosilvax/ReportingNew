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
                url: "{controller}/{action}/{userGuid}/{sessionGuid}/{dbxUrl}/{reportId}",
                defaults: new { controller = "Home",
                    action = "Index",
                    userguid = UrlParameter.Optional,
                    sessionGuid = UrlParameter.Optional,
                    dbxUrl = UrlParameter.Optional,
                    reportId = UrlParameter.Optional }
            );;

            routes.MapRoute(
                name: "getReport",
                url: "{controller}/{action}/{userGuid}/{sessionGuid}/{repid}",
                defaults: new
                {
                    controller = "Home",
                    action = "getReport",
                    userguid = UrlParameter.Optional,
                    sessionGuid = UrlParameter.Optional,
                    repid = UrlParameter.Optional,
                    dbxUrl = UrlParameter.Optional
                }
            );
      //      routes.MapRoute(
      //    name: "LoadForm",
      //    url: "{controller}/{action}/{userGuid}/{sessionGuid}/{repid}/{dbxUrl}",
      //    defaults: new
      //    {
      //        controller = "LoadForm",
      //        action = "getReport",
      //        userguid = UrlParameter.Optional,
      //        sessionGuid = UrlParameter.Optional,
      //        repid = UrlParameter.Optional,
      //        dbxUrl= UrlParameter.Optional
      //    }
      //);
            routes.MapRoute(
                name: "urlJT",
                url: "{controller}/{action}/{userGuid}",
                defaults: new
                {
                    controller = "Home",
                    action = "urlJT",
                    userGuid = UrlParameter.Optional
                }
            );

        }
    }
}

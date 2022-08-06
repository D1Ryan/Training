using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //var settings = new FriendlyUrlSettings();
            //settings.AutoRedirectMode = RedirectMode.Off;
            //routes.EnableFriendlyUrls(settings);
            //全局路由表 忽略掉MVC 对asp.net Web Forms 请求
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.MapRoute(
               "Default",
               "{controller}/{action}/{id}",
               new { controller = "Resource", action = "Index", id = UrlParameter.Optional } // 参数默认值  
           );

            //  RouteTable.Routes.MapRoute(
            //    "Default",
            //    "{controller}/{action}/{id}",
            //    new { controller = "User", action = "Index", id = UrlParameter.Optional } // 参数默认值  
            //);
        }
    }
}

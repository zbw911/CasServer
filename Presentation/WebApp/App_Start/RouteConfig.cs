// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/RouteConfig.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Web.Mvc;
using System.Web.Routing;

namespace CASServer
{
    public class RouteConfig
    {
        #region Class Methods

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("crossdomain.xml");


            routes.MapRoute(
                name: "crossdomain",
                url: "Avatar/crossdomain.xml",
                defaults: new {controller = "Crossdomain", action = "Index", id = UrlParameter.Optional}
                );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }

        #endregion
    }
}
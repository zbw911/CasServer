// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/AdminAreaRegistration.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Web.Mvc;

namespace CASServer.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        #region Instance Properties

        public override string AreaName
        {
            get { return "Admin"; }
        }

        #endregion

        #region Instance Methods

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }

        #endregion
    }
}
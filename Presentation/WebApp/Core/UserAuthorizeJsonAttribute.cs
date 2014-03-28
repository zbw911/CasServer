// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/UserAuthorizeJsonAttribute.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.ComponentModel;
using System.Web.Mvc;
using Application.Dto;

namespace CASServer.Core
{
    public class UserAuthorizeJsonAttribute : ActionFilterAttribute
    {
        #region Instance Properties

        [DefaultValue("未登录")]
        public string UnAuthorizeMessage { get; set; }

        [DefaultValue(-1000)]
        public int UnAuthorizeState { get; set; }

        #endregion

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{

        //    if (filterContext.Result is JsonResult && !filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        var json = new JsonResult
        //                       {
        //                           Data = new BaseState(-1, "未登录"),
        //                           JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //                       };

        //        filterContext.Result = json;
        //    }
        //    base.OnActionExecuting(filterContext);
        //}

        #region Instance Methods

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is JsonResult && !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var json = new JsonResult
                               {
                                   Data = new BaseState(-1, "未登录"),
                                   JsonRequestBehavior = JsonRequestBehavior.AllowGet
                               };

                filterContext.Result = json;
            }
            base.OnActionExecuted(filterContext);
        }

        #endregion
    }
}
// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/HomeController.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Web.Mvc;
using Application.MainBoundedContext.UserModule;

namespace CASServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        #region Instance Methods


        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult About()
        {
            this.ViewBag.Message = "你的应用程序说明页。";

            return this.View();
        }

        public ActionResult Contact()
        {
            this.ViewBag.Message = "你的联系方式页。";

            return this.View();
        }

        public ActionResult Index()
        {
            this.ViewBag.Message = "修改此模板以快速启动你的 ASP.NET MVC 应用程序。";
            //return Content(Url.IsLocalUrl("http://www.xxxxxxx.com").ToString());

            //this._userService.GetRegDateTime(100000);

            return this.View();
        }

        #endregion
    }
}
// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/CASController.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.IO;
using System.Web.Mvc;
using Application.MainBoundedContext.UserModule;
using CASServer.Models;
using Dev.CasServer;
using Dev.CasServer.Authenticator;

namespace CASServer.Controllers
{
    public class CasController : Controller
    {
        #region Readonly & Static Fields

        private readonly CasServer _casServer;

        private readonly IUserService _userService;
        private readonly ICasAuthenticator casAuthenticator;
        private readonly CasServer casServer;
        private static string strJsSDK = null;

        private readonly IUserValidate userValidate;

        #endregion

        #region C'tors

        public CasController(IUserValidate UserValidate, ICasAuthenticator CasAuthenticator, CasServer CasServer,
                             IUserService userService)
        {
            if (UserValidate == null) throw new ArgumentNullException("UserValidate");
            if (CasAuthenticator == null) throw new ArgumentNullException("CasAuthenticator");
            this.casAuthenticator = CasAuthenticator;

            this.userValidate = UserValidate;

            this.casServer = CasServer;
            this._userService = userService;
        }

        #endregion

        #region Instance Methods

        public ActionResult JsSDK()
        {
            return this.JavaScript(this.Js());
        }

        public ActionResult Login(string service)
        {
            this.ViewBag.service = service;
            //如果 serivce 不为空是,有必要返回
            if (!string.IsNullOrEmpty(service))
                this.ViewBag.ReturnUrl = this.Request.RawUrl;

            //Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
            string returl;
            var redrect = this.casServer.HandlePageLoad(service, out returl);


            if (redrect)
            {
                return this.Redirect(returl);
            }

            return this.View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string service)
        {
            this.ViewBag.service = service;

            //if (!WebSecurity.IsConfirmed(model.UserName))
            //{

            //    return RedirectToAction("EmailActivation", "Account", new { email = model.UserName, type = 1 });
            //}


            string redirectUrl;
            // check if this is a CAS request and handle it
            if (this.ModelState.IsValid && this.casServer.HandlePageLogin(
                service, model.UserName, model.Password, model.RememberMe, out redirectUrl))
            {
                if (string.IsNullOrEmpty(redirectUrl))
                {
                    // if not, do it the FormsAuthentication way
                    //FormsAuthentication.RedirectFromLoginPage(model.UserName, model.RememberMe);

                    return this.Redirect(Dev.CasServer.Configuration.CasServerConfiguration.Config.DefaultUrl);
                }
                else
                {
                    return this.Redirect(redirectUrl);
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            //ViewBag.ErrorMessage = "提供的用户名或密码不正确。";
            this.ModelState.AddModelError("", "提供的用户名或密码不正确。");
            return View(model);
        }

        public ActionResult Logout(string service)
        {
            this.casServer.HandleLogoutRequest();
            if (string.IsNullOrEmpty(service))
            {
                return this.RedirectToAction("login");
            }

            return this.Redirect(service);
        }

        public ActionResult ServiceValidate(string service, string ticket)
        {
            var strResponse = this.casServer.HandleServiceValidateRequest(service, ticket);
            return this.Content(strResponse);
        }

        public ActionResult Validate(string service, string ticket)
        {
            var strResponse = this.casServer.HandleValidateRequest(service, ticket);
            return this.Content(strResponse);
        }


        private string Js()
        {
            //if (strJsSDK == null)
            //{
            string content;
            ViewEngineResult view = null;

            view = ViewEngines.Engines.FindPartialView(this.ControllerContext, "JsSDK");
            using (var writer = new StringWriter())
            {
                this.ViewData["basurl"] = Dev.Comm.Web.HttpServerInfo.BaseUrl;
                var context = new ViewContext(this.ControllerContext, view.View, this.ViewData, this.TempData, writer);
                view.View.Render(context, writer);

                writer.Flush();
                content = writer.ToString();
            }

            strJsSDK = content;
            //}
            return strJsSDK;
        }

        #endregion
    }
}
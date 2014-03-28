// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/AccountController.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using Application.MainBoundedContext;
using Application.MainBoundedContext.UserModule;
using CASServer.Core;
using CASServer.Filters;
using CASServer.Models;
using Dev.CasServer.Configuration;
using Dev.Comm;
using Dev.Comm.Web.Mvc.Filter;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace CASServer.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : BaseController
    {
        #region Enums

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        #endregion

        #region Readonly & Static Fields

        private readonly IUserService _userService;

        #endregion

        #region C'tors

        public AccountController(IUserService userService)
        {
            this._userService = userService;
        }

        #endregion

        #region Instance Methods

        [AllowAnonymous, JsonpFilter, UserAuthorizeJson, ActionAllowCrossSiteJson]
        public ActionResult A()
        {
            return this.Json("json", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Activation(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("token can't null");
            }

            //if (WebSecurity.IsConfirmed(email))


            if (this._userService.IsConfirmedByToken(token))
                return this.Message("此帐户已经激活", "/CAS/Login");

            if (WebSecurity.ConfirmAccount(accountConfirmationToken: token))
            {
                this._userService.ConfirmEmail(token);
                return this.Message("激活成功", "/CAS/Login");
            }

            return this.Content("激活失败");
        }

        public ActionResult Binding()
        {
            return this.View();
        }

        [AllowAnonymous, JsonpFilter, UserAuthorizeJson, ActionAllowCrossSiteJson]
        public ActionResult ChangeNick(string nickname)
        {
            var bs = this._userService.ChangeNick(userid: WebSecurity.CurrentUserId, nickname: nickname);
            return this.Json(bs);
        }

        public ActionResult ChangePassword(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess
                    ? "已更改你的密码。"
                    : message == ManageMessageId.SetPasswordSuccess
                          ? "已设置你的密码。"
                          : message == ManageMessageId.RemoveLoginSuccess
                                ? "已删除外部登录。"
                                : "";
            this.ViewBag.HasLocalPassword =
                OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(this.User.Identity.Name));
            this.ViewBag.ReturnUrl = this.Url.Action("Manage");
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(this.User.Identity.Name));
            this.ViewBag.HasLocalPassword = hasLocalAccount;
            this.ViewBag.ReturnUrl = this.Url.Action("ChangePassword",
                                                     new { @in = Dev.Comm.Web.DevRequest.GetInt("in", 0) });
            if (hasLocalAccount)
            {
                if (this.ModelState.IsValid)
                {
                    // 在某些失败方案中，ChangePassword 将引发异常，而不是返回 false。
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(this.User.Identity.Name, model.OldPassword,
                                                                             model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return this.RedirectToAction("ChangePassword",
                                                     new
                                                         {
                                                             Message = ManageMessageId.ChangePasswordSuccess,
                                                             @in = Dev.Comm.Web.DevRequest.GetInt("in", 0)
                                                         });
                    }
                    else
                    {
                        this.ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                    }
                }
            }
            else
            {
                // 用户没有本地密码，因此将删除由于缺少
                // OldPassword 字段而导致的所有验证错误
                var state = this.ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (this.ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(this.User.Identity.Name, model.NewPassword);
                        return this.RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        this.ModelState.AddModelError("", e);
                    }
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        [AllowAnonymous, JsonpFilter, UserAuthorizeJson, ActionAllowCrossSiteJson]
        public ActionResult ChangeSex(int sex)
        {
            var bs = this._userService.ChangeSex(userid: WebSecurity.CurrentUserId, sex: sex);
            return this.Json(bs);
        }


        [AllowAnonymous]
        public bool Check(string username)
        {
            return WebSecurity.UserExists(username);
        }

        [AllowAnonymous]
        public bool CheckNick(string nickname)
        {
            return this._userService.UserNickExist(nickname);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Checktoken(string username, string token)
        {
            var bs = this._userService.CheckPhoneToken(username, token);

            return this.Json(bs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///   验证码，当然要匿名了啊
        /// </summary>
        /// <returns> </returns>
        [AllowAnonymous]
        public ActionResult Code()
        {
            ////生成验证码
            var validateCode = new ValidateCode();
            var code = validateCode.CreateValidateCode(4, 0);
            this.SessionSet(SessionName.验证码, code);
            var bytes = validateCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            var ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // 只有在当前登录用户是所有者时才取消关联帐户
            if (ownerAccount == this.User.Identity.Name)
            {
                // 使用事务来防止用户删除其上次使用的登录凭据
                using (
                    var scope = new TransactionScope(TransactionScopeOption.Required,
                                                     new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    var hasLocalAccount =
                        OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(this.User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(this.User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return this.RedirectToAction("Manage", new { Message = message });
        }

        [AllowAnonymous]
        public ActionResult EmailActivation(string email, int type = 0)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("email can't null");
            }

            this.ViewBag.type = type;
            //if (WebSecurity.IsConfirmed(email))


            if (WebSecurity.IsConfirmed(email))
            {
                return this.Message("此帐户已经激活", "/CAS/Login");
            }


            return this.View(model: email);
        }

        //
        // GET: /Account/Manage

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider,
                                           this.Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var result =
                OAuthWebSecurity.VerifyAuthentication(this.Url.Action("ExternalLoginCallback",
                                                                      new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return this.RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return this.RedirectToCas(returnUrl);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                // 如果当前用户已登录，则添加新帐户
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, this.User.Identity.Name);
                this._userService.InserOrUpdateExtUid(WebSecurity.GetUserId(this.User.Identity.Name));
                return this.RedirectToCas(returnUrl);
            }
            else
            {
                // 该用户是新用户，因此将要求该用户提供所需的成员名称
                var loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                this.ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                this.ViewBag.ReturnUrl = returnUrl;
                return this.View("ExternalLoginConfirmation",
                                 new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (this.User.Identity.IsAuthenticated ||
                !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return this.RedirectToAction("Manage");
            }

            if (this.ModelState.IsValid)
            {
                // 将新用户插入到数据库

                //var user =
                //    db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

                var userprofiles = this._userService.GetUserProfileByUserName(model.UserName.ToLower());
                // 检查用户是否已存在
                if (userprofiles == null)
                {
                    // 将名称插入到配置文件表
                    //db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                    //db.SaveChanges();

                    userprofiles = new Application.Dto.User.UserProfileModel
                                       {
                                           UserName = model.UserName
                                       };
                    bool b = this._userService.AddUserProfiles(userprofiles);

                    OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                    this._userService.InserOrUpdateExtUid(WebSecurity.GetUserId(model.UserName));
                    OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                    return this.RedirectToCas(returnUrl);
                }
                else
                {
                    this.ModelState.AddModelError("UserName", "用户名已存在。请输入其他用户名。");
                }

            }

            this.ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            this.ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return this.View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            return this.PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetPwd()
        {
            return this.View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetPwd(GetPwdModel model)
        {
            var code = (this.SessionGet<string>(SessionName.验证码) ?? "").ToLower();
            this.SessionRemove(SessionName.验证码);

            if (model.Validcode.ToLower() != code)
            {
                this.ModelState.AddModelError("Validcode", "验证码不正确");
                return View(model);
            }

            // 用户没有本地密码，因此将删除由于缺少
            // OldPassword 字段而导致的所有验证错误
            var state = this.ModelState["GetPwdType"];
            if (state != null)
            {
                state.Errors.Clear();
            }

            if (this.ModelState.IsValid)
            {
                var bs = this._userService.GetPassWord(Dev.Comm.Web.HttpServerInfo.BaseUrl, model);

                if (bs.ErrorCode == 0)
                {
                    if (model.GetPwdType == 0)
                        return this.View("_GetPwdMailSucess", model: model.UserName);
                    else
                        return this.View("_GetPwdNext", model: bs.ErrorMessage);
                }
                else
                {
                    if (bs.ErrorCode == -3)
                        return this.Message("此用户还未激活，激活后继续",
                                            this.Url.Action("EmailActivation", new { email = model.UserName }));

                    this.ModelState.AddModelError("", "" + bs.ErrorMessage);
                }
            }


            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            return this.RedirectToAction("Logout", "CAS");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            return this.RedirectToAction("Login", "CAS");
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            return new HttpUnauthorizedResult();
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess
                    ? "已更改你的密码。"
                    : message == ManageMessageId.SetPasswordSuccess
                          ? "已设置你的密码。"
                          : message == ManageMessageId.RemoveLoginSuccess
                                ? "已删除外部登录。"
                                : "";
            this.ViewBag.HasLocalPassword =
                OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(this.User.Identity.Name));
            this.ViewBag.ReturnUrl = this.Url.Action("Manage");
            return this.View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(this.User.Identity.Name));
            this.ViewBag.HasLocalPassword = hasLocalAccount;
            this.ViewBag.ReturnUrl = this.Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (this.ModelState.IsValid)
                {
                    // 在某些失败方案中，ChangePassword 将引发异常，而不是返回 false。
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(this.User.Identity.Name, model.OldPassword,
                                                                             model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return this.RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        this.ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                    }
                }
            }
            else
            {
                // 用户没有本地密码，因此将删除由于缺少
                // OldPassword 字段而导致的所有验证错误
                var state = this.ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (this.ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(this.User.Identity.Name, model.NewPassword);
                        return this.RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        this.ModelState.AddModelError("", e);
                    }
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return this.View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            var code = (this.SessionGet<string>(SessionName.验证码) ?? "").ToLower();
            this.SessionRemove(SessionName.验证码);

            if (model.Validcode.ToLower() != code)
            {
                this.ModelState.AddModelError("Validcode", "验证码不正确");
                return View(model);
            }
            if (this.ModelState.IsValid)
            {
                if (this._userService.UserNickExist(model.NickName))
                {
                    this.ModelState.AddModelError("NickName", "昵称已经被使用");
                    return View(model);
                }

                // 尝试注册用户
                try
                {
                    var token = WebSecurity.CreateUserAndAccount(model.UserName, model.Password,
                                                                 new
                                                                     {
                                                                         Email = model.UserName,
                                                                         model.NickName,
                                                                         model.Sex,
                                                                         model.Province,
                                                                         model.City,
                                                                         model.ProvinceName,
                                                                         model.CityName
                                                                     }, true);

                    var userid = WebSecurity.GetUserId(model.UserName);

                    var uid = this._userService.InserOrUpdateExtUid(userid);


                    SystemMessagerManager.SendValidateMail(Dev.Comm.Web.HttpServerInfo.BaseUrl, model.UserName, model.NickName, "邮件激活",
                                                           SystemMessagerManager.ActMessage(Dev.Comm.Web.HttpServerInfo.BaseUrl, model.UserName,
                                                                                            model.NickName, token));


                    return this.RedirectToAction("EmailActivation", new { email = model.UserName });
                }
                catch (MembershipCreateUserException e)
                {
                    this.ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            var accounts = OAuthWebSecurity.GetAccountsFromUserName(this.User.Identity.Name);
            var externalLogins = new List<ExternalLogin>();
            foreach (var account in accounts)
            {
                var clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                                       {
                                           Provider = account.Provider,
                                           ProviderDisplayName = clientData.DisplayName,
                                           ProviderUserId = account.ProviderUserId,
                                       });
            }

            this.ViewBag.ShowRemoveButton = externalLogins.Count > 1 ||
                                            OAuthWebSecurity.HasLocalAccount(
                                                WebSecurity.GetUserId(this.User.Identity.Name));
            return this.PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResendSms(string username)
        {
            var bs = this._userService.GetPassWord(Dev.Comm.Web.HttpServerInfo.BaseUrl, new GetPwdModel
                                                       {
                                                           UserName = username,
                                                           GetPwdType = 1
                                                       });

            return this.Json(bs, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResendToken(string email)
        {
            if (WebSecurity.IsConfirmed(email))
            {
                return this.Json(false);
            }

            var NickName = this._userService.GetNickNameByUserName(email);

            var token = this._userService.GetTokenByUserName(email);

            SystemMessagerManager.SendValidateMail(Dev.Comm.Web.HttpServerInfo.BaseUrl, email, NickName, "邮件激活",
                                                   SystemMessagerManager.ActMessage(Dev.Comm.Web.HttpServerInfo.BaseUrl, email, NickName, token));

            return this.Json(true);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPwdByPhone(string username, string token)
        {
            this.ViewBag.username = username;
            this.ViewBag.token = token;

            return this.View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPwdByPhone(string username, string token, LocalPasswordModel model)
        {
            this.ViewBag.username = username;
            this.ViewBag.token = token;
            // 用户没有本地密码，因此将删除由于缺少
            // OldPassword 字段而导致的所有验证错误
            var state = this.ModelState["OldPassword"];
            if (state != null)
            {
                state.Errors.Clear();
            }

            if (string.IsNullOrEmpty(username))
                this.ModelState.AddModelError("", "username can't null ");
            if (string.IsNullOrEmpty(token))
                this.ModelState.AddModelError("", "token can't null");


            if (this.ModelState.IsValid)
            {
                var bs = this._userService.ResetPasswordByPhoneToken(token, model.NewPassword, username);

                if (bs.ErrorCode == 0)
                {
                    return this.Message("重置成功", this.Url.Action("Login", "Cas"));
                }
                else
                {
                    this.ModelState.AddModelError("", bs.ErrorMessage);
                }
            }

            return this.View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RestPwd(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return this.Message("不正确定找回密码链接");
            }

            var email = this._userService.GetUserEmailByRestToken(restToken: token);

            if (string.IsNullOrEmpty(email))
                return this.Message("找回密码链接已经过期", this.Url.Action("getpwd"));

            this.ViewBag.Email = email;
            this.ViewBag.token = token;

            return this.View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RestPwd(string token, LocalPasswordModel model)
        {
            // 用户没有本地密码，因此将删除由于缺少
            // OldPassword 字段而导致的所有验证错误
            var state = this.ModelState["OldPassword"];
            if (state != null)
            {
                state.Errors.Clear();
            }

            if (this.ModelState.IsValid)
            {
                var isok =
                    this._userService.ResetPasswordByEmailToken(token, NewPassword: model.NewPassword);

                if (isok)
                    return this.Message("重置密码成功", this.Url.Action("login", "cas"));
                else
                    return this.Message("重置密码失败", this.Url.Action("RestPwd", new { token }));
            }
            return View(model);
        }

        private ActionResult RedirectToCas(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = CasServerConfiguration.Config.DefaultUrl;
                }
                return this.RedirectToAction("Login", "CAS", new { service = returnUrl });
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Class Methods

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请输入其他用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "该电子邮件地址的用户名已存在。请输入其他电子邮件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }

        #endregion

        #region Nested type: ExternalLoginResult

        internal class ExternalLoginResult : ActionResult
        {
            #region C'tors

            public ExternalLoginResult(string provider, string returnUrl)
            {
                this.Provider = provider;
                this.ReturnUrl = returnUrl;
            }

            #endregion

            #region Instance Properties

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            #endregion

            #region Instance Methods

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(this.Provider, this.ReturnUrl);
            }

            #endregion
        }

        #endregion
    }
}
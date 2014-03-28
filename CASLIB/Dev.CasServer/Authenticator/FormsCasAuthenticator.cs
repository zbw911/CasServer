// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月28日 15:34
// 
// 修改于：2013年02月18日 13:52
// 文件名：FormsCasAuthenticator.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.Linq;
using System.Web;
using System.Web.Security;

namespace Dev.CasServer.Authenticator
{
    public class FormsCasAuthenticator : ICasAuthenticator
    {
        #region ICasAuthenticator Members

        public bool CasCheckPermission(string strUserName, string strService)
        {
            var reslut =
               Configuration.CasServerConfiguration.Config.ClientList.OrderByDescending(x => (x.Url ?? "").Length).Any(
                   x => strService.ToLower().IndexOf(x.Url.ToLower()) == 0);
            // no restrictions by default



            return reslut;

        }

        public void CasLogin(string strUserName, bool doRemember)
        {
            // Nothing to do
            //HttpContext.Current.Response.AddHeader("P3P",
            //                  "CP=\"CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR\"");
            //// sign in via forms authentication
            //FormsAuthentication.SetAuthCookie(strUserName, doRemember);
        }

        public void CasLogout()
        {
            // sing out forms authentication
            FormsAuthentication.SignOut();

            // we cannot call FormsAuthentication.RedirectToLoginPage here because it would append a ReturnUrl parameter
            // this.Response.Redirect(FormsAuthentication.LoginUrl);
        }

        public string CasTranslateService(string strService)
        {
            // no additional translation by default
            return strService;
        }


        public bool IsAuthenticated()
        {
            var httpContext = HttpContext.Current;

            return httpContext.User.Identity.IsAuthenticated;
        }


        public string GetName()
        {
            var httpContext = HttpContext.Current;
            return httpContext.User.Identity.Name;
        }

        #endregion
    }
}
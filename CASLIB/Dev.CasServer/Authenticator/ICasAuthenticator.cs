// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月25日 16:11
// 
// 修改于：2013年02月18日 13:52
// 文件名：ICasAuthenticator.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.CasServer.Authenticator
{
    public interface ICasAuthenticator
    {
        #region Instance Methods

        /// <summary>
        ///   The check permission hook will be called when the CAS server receives a CAS login request, after 
        ///   the user has been authenticated and right before the user is redirected back to the client page.
        ///   The hook can check whether the user is permitted to call the requested service and return
        ///   false if not.
        ///   Also, if the client web application provides an interface for external user management, this 
        ///   is the point where a dynamic user creation can be implemented.
        /// </summary>
        /// <param name="strUserName"> </param>
        /// <param name="strService"> </param>
        bool CasCheckPermission(string strUserName, string strService);

        /// <summary>
        ///   The login hook will be called when the CAS server receives a CAS login request, after the check 
        ///   permission hook has been called and right before the user is redirected back to the client page.
        ///   If the server uses forms authentication it should call SetAuthCookie.
        /// </summary>
        void CasLogin(string strUserName, bool doRemember);

        /// <summary>
        ///   The logout hook will be called when the CAS server receives a CAS logout request.
        ///   If the server uses forms authentication it should call SignOut and redirect to the login page.
        /// </summary>
        void CasLogout();

        /// <summary>
        ///   In some scenarious a service translation hook might be useful.
        ///   Some Java CAS client applications tend to request a CAS ticket for a service, and then validate the service under a different name,
        ///   especially when the Java client can be accessed via multiple domains like "www.mycasclient.com" and "wiki.mycasclient.com" the client 
        ///   might request the ticket for "www.mycasclient.com" and then validates the ticket for "wiki.mycasclient.com" (for whatever reason).
        /// </summary>
        /// <param name="strService"> </param>
        /// <returns> </returns>
        string CasTranslateService(string strService);

        /// <summary>
        ///   当前用户名
        /// </summary>
        /// <returns> </returns>
        string GetName();

        /// <summary>
        ///   IsLogin
        /// </summary>
        /// <returns> </returns>
        bool IsAuthenticated();

        #endregion
    }
}
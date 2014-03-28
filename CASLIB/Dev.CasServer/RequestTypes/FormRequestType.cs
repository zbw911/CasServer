// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月28日 17:10
// 
// 修改于：2013年02月18日 13:52
// 文件名：RequestType.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.Web;

namespace Dev.CasServer.RequestTypes
{
    /// <summary>
    ///   这只是对RequestType的一种实现，在实际应用中也仅在WebForm中有意思，对于Mvc就意义不大了。
    ///   added by zbw911 , 2013-1-28
    /// </summary>
    public class FormRequestType : IRequestType
    {
        #region Readonly & Static Fields

        private readonly HttpRequest _httpRequest = HttpContext.Current.Request;

        #endregion

        #region IRequestType Members

        /// <summary>
        ///   Check if the current request is CAS login request.
        /// </summary>
        /// <returns> </returns>
        public bool IsLoginRequest()
        {
            return this._httpRequest.PathInfo == "/login";
        }

        /// <summary>
        ///   Check if the current request is CAS validate (CAS 1.0) request.
        /// </summary>
        /// <returns> </returns>
        public bool IsServiceValidateRequest()
        {
            return this._httpRequest.PathInfo == "/serviceValidate";
        }

        /// <summary>
        ///   Check if the current request is CAS server validate request.
        /// </summary>
        /// <returns> </returns>
        public bool IsValidateRequest()
        {
            return this._httpRequest.PathInfo == "/validate";
        }

        /// <summary>
        ///   Check if the current request is CAS logout request.
        /// </summary>
        /// <returns> </returns>
        public bool IsLogoutRequest()
        {
            //if (IsLoginRequest())
            //{
            //    // when a user logs out some Java CAS client applications (like Confluence) do not call the CAS logout request,
            //    // instead they redirect to the login page, with a parameter indicating that a logout has happened
            //    string strService = httpRequest.QueryString["service"];
            //    return !String.IsNullOrEmpty(strService) && strService.Contains("logout=true");
            //}
            return this._httpRequest.PathInfo == "/logout";
        }

        #endregion
    }
}
// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月28日 17:10
// 
// 修改于：2013年02月18日 13:52
// 文件名：IRequestType.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.CasServer.RequestTypes
{
    /// <summary>
    ///   Created by zbw911
    /// </summary>
    public interface IRequestType
    {
        #region Instance Methods

        /// <summary>
        ///   Check if the current request is CAS login request.
        /// </summary>
        /// <returns> </returns>
        bool IsLoginRequest();

        /// <summary>
        ///   Check if the current request is CAS logout request.
        /// </summary>
        /// <returns> </returns>
        bool IsLogoutRequest();

        /// <summary>
        ///   Check if the current request is CAS validate (CAS 1.0) request.
        /// </summary>
        /// <returns> </returns>
        bool IsServiceValidateRequest();

        /// <summary>
        ///   Check if the current request is CAS server validate request.
        /// </summary>
        /// <returns> </returns>
        bool IsValidateRequest();

        #endregion
    }
}
// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月28日 15:13
// 
// 修改于：2013年02月18日 13:52
// 文件名：IUserValidate.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.CasServer
{
    /// <summary>
    ///   用户验证接口，这个接口去持久化或其它接口进行用户的验证
    /// </summary>
    public interface IUserValidate
    {
        #region Instance Methods

        /// <summary>
        ///   取得用户的扩展属性
        /// </summary>
        /// <param name="strUserName"> </param>
        /// <returns> </returns>
        object GetExtendProperty(string strUserName);

        /// <summary>
        ///   验证用户登录
        /// </summary>
        /// <param name="strUserName"> </param>
        /// <param name="strPassWord"> </param>
        /// <returns> </returns>
        bool PerformAuthentication(string strUserName, string strPassWord, bool rembers, out string ErrorMsg);

        #endregion
    }
}
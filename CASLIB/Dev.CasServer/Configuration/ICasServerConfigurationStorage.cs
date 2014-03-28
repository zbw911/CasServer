// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月29日 10:23
// 
// 修改于：2013年02月18日 13:52
// 文件名：ICasServerConfigurationStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

namespace Dev.CasServer.Configuration
{
    /// <summary>
    ///   配置文件存储接口， zbw911
    /// </summary>
    public interface ICasServerConfigurationStorage
    {
        #region Instance Methods

        /// <summary>
        ///   取得
        /// </summary>
        /// <returns> </returns>
        CasServerConfiguration Get();


        /// <summary>
        ///   根据名称取得
        /// </summary>
        /// <param name="configname"> </param>
        /// <returns> </returns>
        CasServerConfiguration Get(string configname);

        /// <summary>
        ///   保存
        /// </summary>
        /// <param name="config"> </param>
        /// <returns> </returns>
        void Save(CasServerConfiguration config);

        /// <summary>
        /// </summary>
        /// <param name="config"> </param>
        /// <param name="configname"> </param>
        void Save(CasServerConfiguration config, string configname);

        #endregion
    }
}
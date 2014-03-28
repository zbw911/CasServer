// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月29日 10:01
// 
// 修改于：2013年02月18日 13:52
// 文件名：CasServerConfiguration.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Dev.CasServer.Configuration
{
    [DataContract]
    public class CasServerConfiguration
    {
        #region Readonly & Static Fields

        private static CasServerConfiguration _config;
        private static ICasServerConfigurationStorage _configProvider;

        #endregion

        #region Instance Properties

        [DataMember]
        public Client[] ClientList { get; set; }

        /// <summary>
        ///   默认的主页面
        /// </summary>
        [DataMember]
        public string DefaultUrl { get; set; }

        #endregion

        #region Class Properties

        public static CasServerConfiguration Config
        {
            get
            {
                if (_config == null)
                    _config = ConfigProvider.Get();
                return _config;
            }
            set
            {
                _config = value;
                ConfigProvider.Save(_config);
            }
        }

        [XmlIgnore]
        public static ICasServerConfigurationStorage ConfigProvider
        {
            get
            {
                if (_configProvider == null)
                    _configProvider = new XmlCasServerConfigurationStorage();
                return _configProvider;
            }
            set { _configProvider = value; }
        }

        #endregion
    }

    public class Client
    {
        #region Instance Properties

        public string Name { get; set; }
        public string Url { get; set; }

        #endregion
    }
}
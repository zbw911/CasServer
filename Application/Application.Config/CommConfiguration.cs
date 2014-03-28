// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.Config/CommConfiguration.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Application.Config
{
    [DataContract]
    public class CommConfiguration
    {
        #region Readonly & Static Fields

        private static CommConfiguration _config;
        private static IConfigurationStorage _configProvider;

        #endregion

        #region Fields

        /// <summary>
        ///   短信接口地址
        /// </summary>
        [DataMember] public string SmsApi;

        #endregion

        #region Instance Properties

        /// <summary>
        ///   默认的主页面
        /// </summary>
        //[DataMember]
        public string CurrentUrl { get; set; }

        /// <summary>
        ///   XXXXX提供的API
        /// </summary>
        [DataMember]
        public string TuanApibase { get; set; }

        #endregion

        #region Class Properties

        public static CommConfiguration Config
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
        public static IConfigurationStorage ConfigProvider
        {
            get
            {
                if (_configProvider == null)
                    _configProvider = new XmlConfigurationStorage();
                return _configProvider;
            }
            set { _configProvider = value; }
        }

        #endregion
    }
}
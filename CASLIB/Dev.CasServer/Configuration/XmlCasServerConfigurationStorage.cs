// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月29日 11:06
// 
// 修改于：2013年02月18日 13:52
// 文件名：XmlCasServerConfigurationStorage.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using System.IO;
using Dev.Comm.Runtime.Serialization;


namespace Dev.CasServer.Configuration
{
    /// <summary>
    ///   xml保存，911
    /// </summary>
    internal class XmlCasServerConfigurationStorage : ICasServerConfigurationStorage
    {
        #region ICasServerConfigurationStorage Members

        public CasServerConfiguration Get()
        {
            return this.Get(null);
        }

        public CasServerConfiguration Get(string configname)
        {
            CasServerConfiguration instance;
            var xmlFile = GetSettingFile(configname);
            if (File.Exists(xmlFile))
            {
                instance = DataContractSerializationHelper.Deserialize<CasServerConfiguration>(xmlFile);
            }
            else
            {
                instance = new CasServerConfiguration
                               {
                                   ClientList = new Client[] {new Client {Url = "localhost"}},
                                   DefaultUrl = "/"
                               };
                this.Save(instance, configname);
            }

            return instance;
        }

        public void Save(CasServerConfiguration config)
        {
            this.Save(config, null);
        }


        public void Save(CasServerConfiguration config, string configname)
        {
            var settingFile = GetSettingFile(configname);
            DataContractSerializationHelper.Serialize<CasServerConfiguration>(config, settingFile);
        }

        #endregion

        #region Class Methods

        private static string GetSettingFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = "CasServer.config";
            }
            string applicationBaseDirectory = null;
            applicationBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(applicationBaseDirectory, filename);
        }

        #endregion
    }
}
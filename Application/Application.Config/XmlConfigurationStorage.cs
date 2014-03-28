// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.Config/XmlConfigurationStorage.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.IO;
using Dev.Comm.Core.Runtime.Serialization;

namespace Application.Config
{
    /// <summary>
    ///   xml保存，911
    /// </summary>
    internal class XmlConfigurationStorage : IConfigurationStorage
    {
        #region IConfigurationStorage Members

        public CommConfiguration Get()
        {
            return this.Get(null);
        }

        public CommConfiguration Get(string configname)
        {
            CommConfiguration instance;
            var xmlFile = GetSettingFile(configname);
            if (File.Exists(xmlFile))
            {
                instance = DataContractSerializationHelper.Deserialize<CommConfiguration>(xmlFile);
            }
            else
            {
                instance = new CommConfiguration
                               {
                                   //CurrentUrl = "http://caslocal.xxxxxxx.com",
                                   SmsApi = "http://192.168.0.188:9595",
                                   TuanApibase = "http://apilocal.xxxxxxx.com"
                               };
                this.Save(instance, configname);
            }

            return instance;
        }

        public void Save(CommConfiguration config)
        {
            this.Save(config, null);
        }


        public void Save(CommConfiguration config, string configname)
        {
            var settingFile = GetSettingFile(configname);
            DataContractSerializationHelper.Serialize<CommConfiguration>(config, settingFile);
        }

        #endregion

        #region Class Methods

        private static string GetSettingFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename = "Comm.config";
            }
            string applicationBaseDirectory = null;
            applicationBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(applicationBaseDirectory, filename);
        }

        #endregion
    }
}
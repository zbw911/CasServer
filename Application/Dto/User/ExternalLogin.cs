﻿// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.Dto/ExternalLogin.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

namespace CASServer.Models
{
    public class ExternalLogin
    {
        #region Instance Properties

        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }

        #endregion
    }
}
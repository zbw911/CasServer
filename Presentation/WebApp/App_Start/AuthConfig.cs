// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/AuthConfig.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Collections.Generic;
using Dev.DotNetOpenAuth.AspNetExtend.Client;
using Microsoft.Web.WebPages.OAuth;

namespace CASServer
{
    public static class AuthConfig
    {
        #region Class Methods

        public static void RegisterAuth()
        {
            // 若要允许此站点的用户使用他们在其他站点(例如 Microsoft、Facebook 和 Twitter)上拥有的帐户登录，
            // 必须更新此站点。有关详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();

            OAuthWebSecurity.RegisterClient(
                new SinaClient(appId: "11111111", appSecret: "11111111"), displayName: "新浪",
                extraData: new Dictionary<string, object>
                               {
                                   {"class", "sina"}
                               });

            OAuthWebSecurity.RegisterClient(
                new QQClient(appId: "11111111", appSecret: "11111111111"), displayName: "QQ",
                extraData: new Dictionary<string, object>
                               {
                                   {"class", "qq"}
                               });
        }

        #endregion
    }
}
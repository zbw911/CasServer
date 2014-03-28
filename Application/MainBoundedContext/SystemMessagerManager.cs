// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:25
//  文件名：CASServer/Application.MainBoundedContext/SystemMessagerManager.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Configuration;
using Application.Config;

namespace Application.MainBoundedContext
{
    public class SystemMessagerManager
    {
        //private const string indexURL = "http://passport.xxxxxxx.com";

        #region Readonly & Static Fields

        private static readonly string SmsApi = CommConfiguration.Config.SmsApi;
        private static string indexURL = "";

        #endregion

        #region Class Methods

        public static string ActMessage(string baseurl, string email, string nickname, string token)
        {
            indexURL = baseurl;
            if (indexURL.Substring(indexURL.Length - 1) != "/")
            {
                indexURL += "/";
            }

            var ContentMessage = "";
            //激活链接
            var Tourl = indexURL + "Account/Activation?token=" + token;
            ContentMessage +=
                "<table width=100% border=0 bgcolor=#F5F5F5 cellpadding=5 cellspacing=1 style=font-size:12px>";
            ContentMessage += "<tr><td>" + nickname + "，您好!</font></td></tr>";
            ContentMessage += "<tr><td>感谢您注册XXXXX，您的帐号：" + email + "</font></td></tr>";
            ContentMessage += "<tr><td>点击下面链接即可激活您的邮箱：</td></tr>";
            ContentMessage += "<tr><td><a href=" + Tourl + ">" + Tourl + "</a></td></tr>";
            ContentMessage +=
                "<tr><td>(此链接有效时间为48小时，如果无法点击该URL链接地址，请将它复制并粘帖到浏览器的地址输入框，然后单击回车即可。如有疑问,请联系XXXXX客服：010-00000000)</td></tr>";
            ContentMessage += "<tr><td>&nbsp;&nbsp;感谢您对XXXXX的支持，若有疑问请联系客服\a此为系统自动发出，请不要回复本邮件。</td></tr></table>";
            return ContentMessage;
        }

        public static string GetContentForGetPass(string baseurl, string name, string token)
        {
            indexURL = baseurl;
            var ContentMessage = "";

            //激活链接
            var Tourl = indexURL + "/Account/RestPwd?token=" + token;

            //邮件内容
            ContentMessage +=
                "<table width=100% border=0 bgcolor=#F5F5F5 cellpadding=5 cellspacing=1 style=font-size:12px>";
            ContentMessage += "<tr><td>" + name + "，您好\a</font></td></tr>";
            ContentMessage += "<tr><td>您在XXXXX申请了重设密码，请点击下面的链接，然后根据页面提示完成密码重设。</font></td></tr>";
            ContentMessage += "<tr><td>请您在收到此邮件后尽快进行修改密码操作：</td></tr>";
            ContentMessage += "<tr><td><a href=" + Tourl + ">" + Tourl + "</a></td></tr>";
            ContentMessage +=
                "<tr><td>(此链接48小时内有效，如果无法点击该URL链接地址，请将它复制并粘帖到浏览器的地址输入框，然后单击回车即可。如有疑问,请联系XXXXX客服：010-00000000)</td></tr>";
            ContentMessage += "<tr><td>&nbsp;&nbsp;感谢您对XXXXX的支持，若有疑问请联系客服\a此为系统自动发出，请不要回复本邮件。</td></tr></table>";

            return ContentMessage;
        }

        public static string OpenMail(string mail)
        {
            var result = "";
            var str_list = mail.Trim().Split('@');
            if (str_list.Length > 1)
            {
                var mail_ads = str_list[1].Split('.');

                if (mail_ads[0].Equals("qq"))
                {
                    result = "http://mail.qq.com";
                }
                else if (mail_ads[0].Equals("sina"))
                {
                    result = "http://mail.sina.com";
                }
                else if (mail_ads[0].Equals("163"))
                {
                    result = "http://mail.163.com";
                }
                else if (mail_ads[0].Equals("hotmail"))
                {
                    result = "http://hotmail.com";
                }
                else if (mail_ads[0].Equals("live"))
                {
                    result = "http://mail.live.com";
                }
                else if (mail_ads[0].Equals("gmail"))
                {
                    result = "http://mail.google.com";
                }
                else if (mail_ads[0].Equals("126"))
                {
                    result = "http://mail.126.com";
                }
                else if (mail_ads[0].Equals("sohu"))
                {
                    result = "http://mail.sohu.com";
                }
                else if (mail_ads[0].Equals("tom"))
                {
                    result = "http://mail.tom.com";
                }
                else if (mail_ads[0].Equals("21cn"))
                {
                    result = "http://mail.21cn.com";
                }
                else
                {
                    result = "http://mail." + str_list[1].ToString();
                }
            }

            return result;
        }

        /// <summary>
        ///   "尊敬的" + pnum + "，您好！XXXXX发送给您的认证码是" + code + "，请在网站上输入，找回XXXXX密码。如非本人操作，请忽略。"
        /// </summary>
        /// <param name="pnum"> </param>
        /// <param name="message"> </param>
        /// <param name="uid"> </param>
        /// <returns> </returns>
        public static bool SendSMS(string pnum, string message, decimal uid)
        {
            return true;
            var msg = Dev.Comm.Core.Utils.MockUrlCode.UrlEncode(message);
            var sendMessage = Dev.Comm.Net.Http.GetUrl(SmsApi
                                                       + "/api/kt_api.aspx?method=SendMessage&pNum=" + pnum +
                                                       "&message=" + "code" + "&sendmsg=" + (msg) + "&SType=" + "5" +
                                                       "&Uid=" + uid);
            if (sendMessage == "ok") //error表示接口调用成功但是并没有真正发送短信，可能是欠费了
            {
                return true;
            }
            else
            {
                Dev.Log.Loger.Error("Tuanka's Interface Has Error=>" + sendMessage); //调团卡的接口失败，没有正确发送短信
                return false;
            }
        }

        /// <summary>
        ///   发送激活邮箱用的邮件
        /// </summary>
        /// <param name="baseurl"> </param>
        /// <param name="email"> </param>
        /// <param name="nickname"> </param>
        /// <param name="subject"> </param>
        /// <param name="contentMessage"> </param>
        /// <returns> true 成功；false 失败 </returns>
        public static bool SendValidateMail(string baseurl, string email, string nickname, string subject, string contentMessage)
        {
            if (string.IsNullOrEmpty(email))
            {
                //没有输入邮箱参数
                Dev.Log.Loger.Error("【Dx.Activity.BLL.Members--Add()--SendValidateMail()】账号已经注册成功，发送激活邮件失败<没有输入邮箱参数>");
                return false;
            }

            try
            {
                if (!Dev.Comm.Validate.Validate.IsEmail(email))
                {
                    //邮箱格式不正确
                    Dev.Log.Loger.Error("【Dx.Activity.BLL.Members--Add()--SendValidateMail()】账号已经注册成功，发送激活邮件失败<邮箱格式不正确>");
                    return false;
                }
            }
            catch (Exception)
            {
                //邮箱格式不正确
                Dev.Log.Loger.Error(
                    "【Dx.Activity.BLL.Members--Add()--SendValidateMail()】账号已经注册成功，发送激活邮件失败<验证邮箱格式报错了Dev.Comm.Validate.Validate.IsEmail(email)>");

                return false;
            }

            //邮箱设置
            var configMessage = ConfigurationManager.AppSettings["ToMail"].ToString();

            var mymail = new Dev.Comm.Net.Mail("");
            string result;
            var around = mymail.ToSendMail(configMessage, email, nickname, subject, contentMessage, out result);

            if (around != 0)
            {
                Dev.Log.Loger.Error("发送邮件失败,code=" + around + "=>result = " + result);
            }

            return around == 0;
        }

        #endregion
    }
}
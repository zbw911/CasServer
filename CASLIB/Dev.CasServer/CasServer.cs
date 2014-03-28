// ***********************************************************************************
// Created by zbw911 
// 创建于：2013年01月26日 11:01
// 
// 修改于：2013年02月18日 13:52
// 文件名：CasServer.cs
// 
// 如果有更好的建议或意见请邮件至zbw911#gmail.com
// ***********************************************************************************

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Xml;
using Dev.CasServer.Authenticator;
using Dev.Comm.Web;


namespace Dev.CasServer
{
    /// <summary>
    ///   CAS server class that provides the basic Jasig CAS functionality.
    /// </summary>
    public class CasServer
    {
        #region Readonly & Static Fields

        private readonly IUserValidate _userValidate;
        private readonly ICasAuthenticator _casAuthenticator;

        #endregion

        #region C'tors

        public CasServer(IUserValidate userValidate, ICasAuthenticator casAuthenticator)
        {
            this._userValidate = userValidate;
            this._casAuthenticator = casAuthenticator;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        ///   Handle CAS logout request: logout and redirect to the logon page.
        /// </summary>
        public void HandleLogoutRequest()
        {
            // call the logout hook
            this._casAuthenticator.CasLogout();
        }

        /// <summary>
        ///   处理登录页面加载
        /// </summary>
        /// <param name="strService"> </param>
        /// <param name="returl"> </param>
        /// <returns> 是否有必要进行跳转 </returns>
        public bool HandlePageLoad(string strService, out string returl)
        {
            //用户已经登录了
            if (this._casAuthenticator.IsAuthenticated())
            {
                // when the user is already authenticated, then directly redirect to the requested service
                returl = this.HandleLoginRequest(strService, this._casAuthenticator.GetName(), false);
                return !string.IsNullOrEmpty(returl);
            }

            returl = "";
            return false;
        }


        /// <summary>
        ///   Handle Login_Click from Login page, after a successfull authentication
        /// </summary>
        /// <param name="strService"> </param>
        /// <param name="strUserName"> </param>
        /// <param name="strPassWord"> </param>
        /// <param name="doRemember"> </param>
        /// <param name="redirectUrl"> </param>
        /// <returns> Returns true when the call was handled. </returns>
        public bool HandlePageLogin(string strService, string strUserName, string strPassWord, bool doRemember,
                                    out string redirectUrl)
        {
            redirectUrl = "";
            string errorMsg;
            // validate user name and password
            if (this._userValidate.PerformAuthentication(strUserName, strPassWord, doRemember, out errorMsg))
            {
                // call the login hook ， 是否已经设置了cookie?设置登录的cookies
                this._casAuthenticator.CasLogin(strUserName, doRemember);

                redirectUrl = this.HandleLoginRequest(strService, strUserName, doRemember);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///   处理 cas/ServiceValidate 的请求
        /// </summary>
        /// <param name="service"> </param>
        /// <param name="ticket"> </param>
        /// <returns> </returns>
        public string HandleServiceValidateRequest(string service, string ticket)
        {
            var httpContext = HttpContext.Current;
            // ticket service validation CAS 2.0
            string strUserName, strErrorCode, strErrorMsg;
            var isOK = this.ValidateTicket(service, ticket, out strUserName, out strErrorCode, out strErrorMsg);

            var strResponse = this.BuildServiceValidateResponse(isOK, strUserName, strErrorCode, strErrorMsg);


            return strResponse;
        }

        public string HandleValidateRequest(string service, string ticket)
        {
            // ticket validation CAS 1.0
            string strUserName, strErrorCode, strErrorMsg;
            var isOk = this.ValidateTicket(service, ticket, out strUserName, out strErrorCode, out strErrorMsg);

            var strResponse = this.BuildValidateResponse(isOk, strUserName);

            return strResponse;
        }


        /// <summary>
        ///   Build a XML response for ticket service validation CAS 2.0
        /// </summary>
        /// <param name="isOk"> Indicates whether it is a positive response </param>
        /// <param name="strUserName"> The user name to be included in the response. </param>
        /// <param name="strErrorCode"> The error code to be included in the response. </param>
        /// <param name="strErrorMsg"> The error message to be included in the response. </param>
        /// <returns> </returns>
        private string BuildServiceValidateResponse(bool isOk, string strUserName, string strErrorCode,
                                                    string strErrorMsg)
        {
            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Encoding = Encoding.UTF8;
            //settings.Indent = true;

            var writer = new StringWriter();
            var xmlwriter = new XmlTextWriter(writer);
            xmlwriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
            xmlwriter.WriteStartElement("cas:serviceResponse");
            xmlwriter.WriteAttributeString("xmlns:cas", "http://www.yale.edu/tp/cas");
            if (isOk)
            {
                xmlwriter.WriteStartElement("cas:authenticationSuccess");
                xmlwriter.WriteStartElement("cas:user");
                xmlwriter.WriteString(strUserName);
                xmlwriter.WriteEndElement();

                //下面是扩展属性，added by zbw911 11:41
                var extobj = this._userValidate.GetExtendProperty(strUserName);

                if (extobj != null)
                {
                    xmlwriter.WriteStartElement("cas:ext");

                    foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(extobj))
                    {
                        xmlwriter.WriteStartElement("cas:" + propertyDescriptor.Name);
                        var value = propertyDescriptor.GetValue(extobj);
                        xmlwriter.WriteString(value == null ? "" : value.ToString());
                        xmlwriter.WriteEndElement();
                    }
                    xmlwriter.WriteEndElement();
                }


                xmlwriter.WriteEndElement();
            }
            else
            {
                xmlwriter.WriteStartElement("cas:authenticationFailure");
                xmlwriter.WriteString(strErrorMsg);
                xmlwriter.WriteEndElement();
            }
            xmlwriter.WriteEndElement();

            xmlwriter.Close();
            return writer.ToString();
        }

        /// <summary>
        ///   Build a response for ticket validation CAS 1.0
        /// </summary>
        /// <param name="isOk"> Indicates whether it is a positive response </param>
        /// <param name="strUserName"> The user name to be included in the response. </param>
        /// <returns> </returns>
        private string BuildValidateResponse(bool isOk, string strUserName)
        {
            return isOk ? "yes\n" + strUserName + "\n" : "no\n\n";
        }

        /// <summary>
        ///   Handle CAS login request: create a ticket and redirect to the requested service.
        /// </summary>
        /// <param name="strService"> </param>
        /// <param name="strUserName"> </param>
        /// <param name="doRemember"> </param>
        private string HandleLoginRequest(string strService, string strUserName, bool doRemember)
        {
            var strRedirectUrl = strService;

            if (!Dev.Comm.Web.UrlHelper.IsCurrentDomainUrl(strService) && !UrlHelper.IsCurrentDomainUrl(strService))
            {
                //取得Service
                if (String.IsNullOrEmpty(strService)) return "";

                // translate the service
                strService = this.TranslateService(strService);

                // call the check permission hook
                if (!this._casAuthenticator.CasCheckPermission(strUserName, strService))

                    throw new ClientNoPermissionException("无权限的接入端：" + strService);
                //return "";


                // create the CAS ticket based on user name and service
                var strTicket = CasTicket.Issue(strUserName, strService);

                // build the redirection url

                strRedirectUrl += strService.IndexOf('?') == -1 ? "?ticket=" : "&ticket=";
                strRedirectUrl += strTicket;
            }

            return strRedirectUrl;
            // redirect to the requested service
            // httpContext.Response.Redirect(strRedirectUrl);
        }

        /// <summary>
        ///   Service translation
        /// </summary>
        /// <param name="strService"> </param>
        /// <returns> </returns>
        private string TranslateService(string strService)
        {
            // ignore any jsessionid in the service string, because the Java CAS client tends to request a CAS ticket for a service,
            // and then validates that service including a jsessionid (for whatever reason)
            var nIdx = strService.IndexOf(';');
            if (nIdx != -1) strService = strService.Substring(0, nIdx);

            // call service translation hook
            strService = this._casAuthenticator.CasTranslateService(strService);

            return strService;
        }

        /// <summary>
        ///   Validate the given ticket and return the corresponding user, or an error code and message
        /// </summary>
        /// <param name="strTicketRequest"> </param>
        /// <param name="strUserName"> </param>
        /// <param name="strErrorCode"> </param>
        /// <param name="strErrorMsg"> </param>
        /// <param name="strServiceRequest"> </param>
        /// <returns> Returns true when the ticket is valid. </returns>
        private bool ValidateTicket(string strServiceRequest, string strTicketRequest, out string strUserName,
                                    out string strErrorCode, out string strErrorMsg)
        {
            strUserName = strErrorCode = strErrorMsg = "";

            // Documentation: http://www.jasig.org/cas/protocol

            //string strServiceRequest = httpContext.Request.QueryString["service"];
            if (String.IsNullOrEmpty(strServiceRequest))
            {
                strErrorCode = "INVALID_REQUEST";
                Trace.TraceError("CAS validate request violation - service missing");
                return false;
            }
            // string strTicketRequest = httpContext.Request.QueryString["ticket"];
            if (String.IsNullOrEmpty(strTicketRequest))
            {
                strErrorCode = "INVALID_REQUEST";
                Trace.TraceError("CAS validate request violation - ticket missing");
                return false;
            }

            var strService = "";
            strUserName = CasTicket.CheckAndPunch(strTicketRequest, ref strService);
            if (strUserName == "")
            {
                strErrorCode = "INVALID_TICKET";
                strErrorMsg = "CAS ticket violation - requested service '" + strServiceRequest + "'";
                Trace.TraceError(strErrorMsg);
                return false;
            }
            strServiceRequest = this.TranslateService(strServiceRequest);
            if (strService != strServiceRequest)
            {
                strErrorCode = "INVALID_SERVICE";
                strErrorMsg = "CAS service violation - requested service '" + strServiceRequest + "' aviable service '" +
                              strService + "' user '" + strUserName + "'";
                Trace.TraceError(strErrorMsg);
                return false;
            }

            return true;
        }

        #endregion
    }
}
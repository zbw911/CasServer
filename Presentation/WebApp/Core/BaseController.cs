// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/BaseController.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Collections.Generic;
using System.Web.Mvc;
using Dev.Comm.Web;
using Dev.Comm.Web.Mvc.Filter;
using ShareSession;

namespace CASServer.Core
{
    [ErrorFilter]
    public class BaseController : Controller
    {
        #region Instance Methods

        public T SessionGet<T>(SessionName session)
        {
            return SessionOperateBase.Get<T>(session.GetType().FullName + session.ToString());
        }

        public void SessionRemove(SessionName session)
        {
            SessionOperateBase.Remove(session.GetType().FullName + session.ToString());
        }

        public void SessionSet(SessionName session, object value)
        {
            SessionOperateBase.Set(session.GetType().FullName + session.ToString(), value);
        }

        /// <summary>
        ///   提示消息
        /// </summary>
        /// <param name="messagelist"> </param>
        /// <param name="redirectto"> </param>
        /// <param name="to_title"> </param>
        /// <param name="time"> </param>
        /// <param name="return_msg"> </param>
        /// <returns> </returns>
        protected ActionResult Message(List<string> messagelist, string redirectto = "", string to_title = "跳转",
                                       int time = 3, string return_msg = "")
        {
            if (redirectto == "") redirectto = HttpServerInfo.GetUrlReferrer();
            var model = new Messager
                            {
                                MessageList = messagelist,
                                redirectto = redirectto,
                                to_title = to_title,
                                time = time,
                                return_msg = return_msg
                            };
            return View("_Messager", model);
        }


        /// <summary>
        ///   提示消息
        /// </summary>
        /// <param name="strmessage"> </param>
        /// <param name="redirectto"> </param>
        /// <param name="to_title"> </param>
        /// <param name="time"> </param>
        /// <param name="return_msg"> </param>
        /// <returns> </returns>
        protected ActionResult Message(string strmessage, string redirectto = "", string to_title = "跳转", int time = 3,
                                       string return_msg = "")
        {
            return this.Message(new List<string> {strmessage}, redirectto, to_title, time, return_msg);
        }

        #endregion
    }
}
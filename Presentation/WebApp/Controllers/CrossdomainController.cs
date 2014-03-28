// ***********************************************************************************
//  Created by zbw911 
//  创建于：2013年06月03日 16:48
//  
//  修改于：2013年06月03日 17:24
//  文件名：CASServer/WebApp/CrossdomainController.cs
//  
//  如果有更好的建议或意见请邮件至 zbw911#gmail.com
// ***********************************************************************************

using System.Web.Mvc;

namespace CASServer.Controllers
{
    public class CrossdomainController : Controller
    {
        //
        // GET: /Crossdomain/

        #region Instance Methods

        public ActionResult Index()
        {
            var xml = @"<?xml version=""1.0""?>
<cross-domain-policy>
  <allow-access-from domain=""*"" />
</cross-domain-policy>";


            return new ContentResult
                       {
                           Content = xml,
                           ContentType = "text/xml"
                       };
            //return View();            //return View();
        }

        #endregion
    }
}
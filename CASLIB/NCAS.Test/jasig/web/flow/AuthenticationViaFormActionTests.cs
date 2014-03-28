using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCAS.Test.Mock;
using NCAS.jasig;
using NCAS.jasig.authentication.principal;
using NCAS.jasig.web.flow;
using NCAS.jasig.web.support;
using Ninject;

namespace NCAS.Test.jasig.web.flow
{
    [TestClass]
    public class AuthenticationViaFormActionTests
    {
        private AuthenticationViaFormAction _authenticationViaFormAction;
        private CentralAuthenticationService _centralAuthenticationService;//= new CentralAuthenticationServiceImpl();
        [TestInitialize]
        public void init()
        {
            //MockHttp.FakeHttpContext("lt=loginticket");

            //HttpContext.Current.Session["loginTicket"] = "loginticket";
            //HttpContext.Current.Request.Form["lt"] = "loginticket";
            //_authenticationViaFormAction = NinectInit.Kernel.Get<AuthenticationViaFormAction>();
            this._centralAuthenticationService = NinectInit.Kernel.Get<CentralAuthenticationService>();



            _authenticationViaFormAction = NinectInit.Kernel.Get<AuthenticationViaFormAction>();//.setCentralAuthenticationService(this._centralAuthenticationService);

            //HttpContext.Current.Session["loginTicket"] = "";
        }

        [TestMethod]
        public void TestMethod1()
        {

            MockHttp.FakeHttpContext("");

            Credentials credentials = new NCAS.jasig.authentication.principal.UsernamePasswordCredentials
                                          {

                                          };
            ((UsernamePasswordCredentials)credentials).setPassword("test");
            ((UsernamePasswordCredentials)credentials).setUsername("test");

            _authenticationViaFormAction.submit(HttpContext.Current, credentials, null);
        }

        

        [TestMethod]
        public void MyTestMethodWithService()
        {
            MockHttp.FakeHttpContext("Service=http://www.google.com");

            Credentials credentials = new NCAS.jasig.authentication.principal.UsernamePasswordCredentials
            {

            };
            ((UsernamePasswordCredentials)credentials).setPassword("test");
            ((UsernamePasswordCredentials)credentials).setUsername("test");

            if ("success" == _authenticationViaFormAction.submit(HttpContext.Current, credentials, null))
            {
                //这里还有些问题，还差一个  init
                var service = WebUtils.getService(HttpContext.Current).getResponse("").getUrl();

                Console.WriteLine(service);
            }
        }
    }
}

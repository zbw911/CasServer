using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NCAS.Test.Mock
{
    [TestClass]
    class MockHttp
    {
        ////[AssemblyInitialize]
        //public static void init(TestContext context)
        //{
        //    HttpContext.Current = FakeHttpContext();


        //}

        public static HttpContext FakeHttpContext(string queryString, string host = "http://127.0.0.1")
        {
            var httpRequest = new HttpRequest("", host, queryString);
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                        null, CallingConventions.Standard,
                                        new[] { typeof(HttpSessionStateContainer) },
                                        null)
                                .Invoke(new object[] { sessionContainer });

            HttpContext.Current = httpContext;

            return httpContext;
        }
    }
}

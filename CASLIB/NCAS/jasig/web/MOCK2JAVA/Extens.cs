using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace NCAS.jasig.web.MOCK2JAVA
{
    static class Extens
    {
        //HttpContext

        public static NameValueCollection getRequestParameters(this HttpContext context)
        {
            return context.Request.Params;
        }

        public static string get(this NameValueCollection nvc, string key)
        {
            return nvc[key];
        }
    }
}

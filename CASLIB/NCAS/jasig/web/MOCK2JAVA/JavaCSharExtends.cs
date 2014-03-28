using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NCAS.jasig.web.MOCK2JAVA
{
    static class JavaCSharExtends
    {
        public static string getParameter(this  HttpRequest request, string parms)
        {
            return request.QueryString[parms];
        }



        public static string getRemoteAddr(this  HttpRequest request)
        {
            //return Dev.Comm.

            return "clientip";
            //return request.QueryString[parms];
        }
    }
}

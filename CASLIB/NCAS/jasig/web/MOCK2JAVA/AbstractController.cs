using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NCAS.jasig.web
{
    public class AbstractController
    {
        public ModelAndView handleRequest(HttpRequest request, HttpResponse response)
        {
            throw new System.NotImplementedException();
        }

        protected void setCacheSeconds(int i)
        {
            throw new NotImplementedException();
        }
    }
}

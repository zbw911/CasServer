using System;
using NCAS.jasig.validation;

namespace NCAS.jasig.web.MOCK2JAVA
{
    public class ServletRequestDataBinder
    {
        private ValidationSpecification validationSpecification;
        private string p;

        public ServletRequestDataBinder(ValidationSpecification validationSpecification, string p)
        {
            // TODO: Complete member initialization
            this.validationSpecification = validationSpecification;
            this.p = p;
        }
        public void setRequiredFields(string renew)
        {
            throw new NotImplementedException();
        }

        internal void bind(System.Web.HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}

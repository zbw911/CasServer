using System;
using System.Collections.Generic;

namespace NCAS.jasig.authentication.principal.support
{
    class SimplePrincipal : Principal
    {
        private string principalId;
        private Dictionary<string, object> convertedAttributes;

        public SimplePrincipal(string principalId)
        {
            // TODO: Complete member initialization
            this.principalId = principalId;
        }

        public SimplePrincipal(string principalId, Dictionary<string, object> convertedAttributes)
        {
            // TODO: Complete member initialization
            this.principalId = principalId;
            this.convertedAttributes = convertedAttributes;
        }

        public string getId()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> getAttributes()
        {
            throw new NotImplementedException();
        }
    }
}

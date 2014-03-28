using System;
using System.Collections.Generic;
using NCAS.jasig.services.persondir;

namespace NCAS.jasig.authentication.principal.support
{
    class StubPersonAttributeDao : IPersonAttributeDao
    {
        private Dictionary<string, List<object>> dictionary;

        public StubPersonAttributeDao(Dictionary<string, List<object>> dictionary)
        {
            // TODO: Complete member initialization
            this.dictionary = dictionary;
        }

        public IPersonAttributes getPerson(string uid)
        {
            //throw new NotImplementedException();

            return null;

        }

        public HashSet<IPersonAttributes> getPeople(Dictionary<string, object> query)
        {
            throw new NotImplementedException();
        }

        public HashSet<IPersonAttributes> getPeopleWithMultivaluedAttributes(Dictionary<string, List<object>> query)
        {
            throw new NotImplementedException();
        }

        public HashSet<string> getPossibleUserAttributeNames()
        {
            throw new NotImplementedException();
        }

        public HashSet<string> getAvailableQueryAttributes()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<object>> getMultivaluedUserAttributes(Dictionary<string, List<object>> seed)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<object>> getMultivaluedUserAttributes(string uid)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> getUserAttributes(Dictionary<string, object> seed)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> getUserAttributes(string uid)
        {
            throw new NotImplementedException();
        }
    }
}

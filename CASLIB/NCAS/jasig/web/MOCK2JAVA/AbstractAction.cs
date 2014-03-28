using System;
using NCAS.jasig.web.flow;

namespace NCAS.jasig.web.MOCK2JAVA
{
    public class AbstractAction
    {
        protected Event result(string p)
        {
            throw new NotImplementedException();
        }

        protected Event error()
        {
            throw new NotImplementedException();
        }

        protected Event success()
        {
            throw new NotImplementedException();
        }
    }
}

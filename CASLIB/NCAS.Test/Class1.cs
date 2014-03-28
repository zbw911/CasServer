using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCAS.Test
{

    interface IDyn<T>
    {
        List<T> Get();

        dynamic OK();
    }

    class Dyn : IDyn
    {
        public List<string> Get()
        {
            return new List<string>()
            {
                "a","b","c"
            };
        }

        public List<string> OK()
        {
            //return "A" as dynamic;

            return null;
        }
    }

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class MyTestClass
    {
        IDyn id = new Dyn();

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void MyTestMethod()
        {


            List<string> ids = id.Get();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void MyTestMethod2()
        {

        }

    }
}

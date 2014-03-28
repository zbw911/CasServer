using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace NCAS.Test.LT
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ker = new Ninject.StandardKernel();

            //ker.Bind<II>().To<A>().Named("a");
            //ker.Bind<II>().To<A>().When(x=>x);
            ker.Bind<II>().To<A>().When(x => x.Target.Name == "a");

            ker.Bind<II>().To<B>().When(x => x.Target.Name == "b");

            //ker.Bind<All>().To<All>().WithConstructorArgument("a", new A()).WithConstructorArgument("b", new B());

            var all = ker.Get<All>();

            all.Print();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace NCAS.Test.LT
{
    public class All
    {
        private readonly II _a;
        private readonly II _b;

        public All( II a, II b)
        {
            _a = a;
            _b = b;
        }


        public void Print()
        {
            Console.WriteLine(_a.get());

            Console.WriteLine(_b.get());
        }
    }


    public interface II
    {
        string get();
    }

    class A : II
    {
        public string get()
        {
            return "a";
        }
    }

    class B : II
    {
        public string get()
        {
            return "b";
        }
    }
}

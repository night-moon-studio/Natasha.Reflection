using System;
using System.Collections.Generic;
using System.Text;

namespace UTProject
{
    public abstract class TestModelBase
    {
        public string Name;
        public abstract string oviField { get; set; }
    }
    public abstract class TestModel : TestModelBase
    {
        public int Age;
        public ref int A()
        {
            return ref Age;
        }

        public new string Name;
        public string Name2;
        public abstract string abField { get; set; }
        public virtual string viField { get; set; }
        public override string oviField { get; set; }
    }
}

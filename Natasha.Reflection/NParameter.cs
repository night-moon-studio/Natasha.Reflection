using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Reflection
{
    public class NParameter
    {

        public readonly bool IsOut;
        public readonly bool IsIn;
        public readonly bool IsRef;
        public readonly bool IsReadonly;
        public readonly Type ParameterType;
        public readonly string Name;
        public NParameter(bool isOut, bool isIn, bool isRef, bool isReadonly,string name, Type type)
        {
            IsOut = isOut;
            IsIn = isIn;
            IsRef = isRef;
            IsReadonly = isReadonly;
            Name = name;
            ParameterType = type;
        }
    }
}

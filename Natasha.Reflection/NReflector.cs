using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Reflection
{
    public class NReflector
    {

        private readonly Type _type;
        private NReflector(Type type)
        {
            _type = type;
        }


        public static NReflector Create(Type type)
        {
            return new NReflector(type);
        }


        public static NReflector Create<T>()
        {
            return new NReflector(typeof(T));
        }



    }

}

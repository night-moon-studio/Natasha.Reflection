using System;
using System.Reflection;
using UTProject;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            var field = typeof(TestModel).GetMethod("A");
            
            var field1 = typeof(TestModel).GetProperty("viField");
            Console.WriteLine(field.DeclaringType.BaseType.GetField(field.Name) !=null);
            Console.WriteLine(field1.DeclaringType.BaseType.GetField(field1.Name) != null);
            Console.ReadKey();
        }
    }
}

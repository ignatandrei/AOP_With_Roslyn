using AOPMethodsCommon;
using System;

namespace AOPMethodsTest
{
    [AutoMethods()]
    class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        private string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}

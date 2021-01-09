using System;

namespace AOPMethodsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Person p = new();
            p.FirstName = "Andrei";
            p.LastName = "Ignat";
            Console.WriteLine(p.FullName());

        }
    }
}

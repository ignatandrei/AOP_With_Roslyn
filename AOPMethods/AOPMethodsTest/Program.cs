using System;
using System.Threading.Tasks;

namespace AOPMethodsTest
{
    class Program
    {
        
        static async Task<int> Main(string[] args)
        {
            
            Person p = new();
            p.FirstName = "Andrei";
            p.LastName = "Ignat";
            p.WriteToConsoleFullName();

            Console.WriteLine("");
            Console.WriteLine(await p.TestWithParam(10));
            return 0;

        }
    }
}

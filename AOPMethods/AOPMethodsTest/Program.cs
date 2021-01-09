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
            Console.WriteLine(await p.MyTest());
            return 0;

        }
    }
}

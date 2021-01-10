using System;
using System.Threading.Tasks;

namespace AOPMethodsTest
{
    class Program
    {
        
        static async Task<int> Main(string[] args)
        {
            var x = enumTest2.idTest2();
            long y = 7;
            var s = y.ParseExactTest2();
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

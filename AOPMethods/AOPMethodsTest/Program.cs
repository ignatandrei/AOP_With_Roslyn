using System;
using System.Threading.Tasks;

namespace AOPMethodsTest
{
    enum x
    {
        a=0
    }
    class Program
    {

        public  const string a = "asda";
        
        static async Task<int> Main(string[] args)
        {
            
            
            Console.WriteLine(Person_Metadata.LastName);
            Console.WriteLine(Person_EnumProps.FirstName);
            //var x = enumTest2.idTest2();
            long y = 7;
            var s = y.ParseExactTest2();
            var q=y.ToString().ParseExactTest2();
            var s1 = s.ToString().ParseExactTest2();
            Person p = new();
            p.FirstName = "Andrei";
            p.LastName = "Ignat";
            p.WriteToConsoleFullName();
            Console.WriteLine("");
            Console.WriteLine(await p.TestWithParam(10));
            Console.Write(await p.TaskWithData("andrei"));
            await p.TaskWithDat();
            Console.ReadKey();
            return 0;

        }
    }
}

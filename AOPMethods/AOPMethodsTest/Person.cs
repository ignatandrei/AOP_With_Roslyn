using AOPMethodsCommon;
using System;
using System.Threading.Tasks;

namespace AOPMethodsTest
{
    [AutoMethods(template = TemplateMethod.MethodWithPartial, MethodPrefix ="pub", MethodSuffix ="bup")]
    partial class Person
    {
        partial void Method_Start(string methodName)
        {
            Console.WriteLine($"start {methodName}");
        }
        partial void Method_End(string methodName)
        {
            Console.WriteLine($"end {methodName}");
        }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        private string pubFullName()
        {
            
            return FirstName + " " + LastName;
        }

        private void pubWriteToConsoleFullName()
        {
            Console.WriteLine(this.FullName());
        }
        private async Task<string> MyTestbup()
        {
            await Task.Delay(1000);
            return FullName();
        }

        private async Task<string> pubTestWithParam(int s)
        {
            await Task.Delay(1000);
            var data = await MyTest();
            return "testWithParam:" + s + data;

        }


    }
}

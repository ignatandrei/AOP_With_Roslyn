using AOPMethodsCommon;
using System;
using System.Threading.Tasks;

namespace AOPMethodsTest
{
    [AutoMethods(template = TemplateMethod.MethodWithPartial, MethodPrefix ="pub")]
    [AutoMethods(template = TemplateMethod.CustomTemplateFile,CustomTemplateFileName ="privateTryCatch.txt",  MethodSuffix = "bup")]
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
        private Task<int> pubTaskWithData(string s)
        {
            return Task.FromResult((s?.Length ?? 0));
        }
        private Task pubTaskWithDat()
        {
            
            Console.Write("test");
            return Task.CompletedTask;
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

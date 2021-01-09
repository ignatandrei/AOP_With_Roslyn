using AOPMethodsCommon;
using System;
using System.Threading.Tasks;

namespace AOPMethodsTest
{
    [AutoMethods(template = TemplateMethod.CallerAtttributes, MethodPrefix ="pub", MethodSuffix ="bup")]
    partial class Person
    {
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

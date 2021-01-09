using AOPMethodsCommon;
using System.Threading.Tasks;

namespace AOPMethodsTest
{
    [AutoMethods(template = TemplateMethod.TryCatchConsole, MethodPrefix ="pub", MethodSuffix ="bup")]
    partial class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        private string pubFullName()
        {
            
            return FirstName + " " + LastName;
        }
        //[System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
        private async Task<string> MyTestbup()
        {
            await Task.Delay(1000);
            return FirstName + " " + LastName;
        }


    }
}

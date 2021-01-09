using AOPMethodsCommon;

namespace AOPMethodsTest
{
    [AutoMethods(template = TemplateMethod.TryCatchConsole, MethodPrefix ="pub")]
    partial class Person
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        private string pubFullName()
        {
            
            return FirstName + " " + LastName;
        }


     
    }
}

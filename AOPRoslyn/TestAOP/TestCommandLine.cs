using AOPRoslyn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestAOP
{
    [TestClass]
    public class TestCommandLine
    {
        [TestMethod]
        public void TestMethodRewriter()
        {

            var rc = new RewriteCode();
            rc.Code = @"
using System;
namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
              var dt=DateTime.Now;
        }
     }
}";
            var result= rc.RewriteCodeMethod();
            var newCode = @"
using System;

namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" + "\"Program_Main_6\"" + @");//this is automatically added
            var dt = DateTime.Now;
        }
    }
}";
            Assert.AreEqual(newCode.Replace(Environment.NewLine,""), newCode.Replace(Environment.NewLine, ""));
        }
    }
}

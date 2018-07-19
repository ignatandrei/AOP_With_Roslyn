using AOPRoslyn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestAOP
{
    [TestClass]
    public class TestLastLine
    {
        [TestMethod]
        public void TestMethodRewriterLastLine()
        {

            var rc = new RewriteCode(
                formatterFirstLine: "Console.WriteLine(\"start {nameClass}_{nameMethod}_{lineStartNumber}\");",
                formatterLastLine: "Console.WriteLine(\"end {nameClass}_{nameMethod}_{lineStartNumber}\");"
                );
            rc.PreserveLinesNumber = false;
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
            var result = rc.RewriteCodeMethod();
            var newCode = @"
using System;

namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" + "\"start Program_Main_6\"" + @");
            var dt = DateTime.Now;
            Console.WriteLine(" + "\"end Program_Main_6\"" + @");
        }
    }
}";
            Assert.AreEqual(result.Replace(Environment.NewLine, ""), newCode.Replace(Environment.NewLine, ""));
        }


       
    }
}
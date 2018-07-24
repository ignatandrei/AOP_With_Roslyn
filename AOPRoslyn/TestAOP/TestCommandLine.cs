using AOPRoslyn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestAOP
{
    [TestClass]
    public class TestCommandLine
    {
        [TestMethod]
        public void TestMethodRewriterSimple()
        {

            var rc = new RewriteCode();
            rc.Options.PreserveLinesNumber = false;
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
            Console.WriteLine(" + "\"startProgram_Main_6\"" + @");
            var dt = DateTime.Now;
            Console.WriteLine(" + "\"endProgram_Main_6\"" + @");
        }
    }
}";
            Assert.AreEqual(result.Replace(" ", "").Replace(Environment.NewLine, ""), newCode.Replace(" ", "").Replace(Environment.NewLine, ""));
        }


        [TestMethod]
        public void TestMethodRewriterAddVariable()
        {

            var rc = new RewriteCode(
                new AOPFormatter()
                {
                     FormatterFirstLine= "string s=\"this is method {nameMethod} from class {nameClass} at line {lineStartNumber}\";",
                     FormatterLastLine = null
                }
                );
            rc.Options.PreserveLinesNumber = false;
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
            string s = ""this is method Main from class Program at line 6"";
            var dt = DateTime.Now;
        }
    }
}";
            Assert.AreEqual(result.Replace(" ", "").Replace(Environment.NewLine, ""), newCode.Replace(" ", "").Replace(Environment.NewLine, ""));
        }
    }
}
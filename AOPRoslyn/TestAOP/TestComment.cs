using AOPRoslyn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestAOP
{
    [TestClass]
    public class TestComment
    {
        [TestMethod]
        public void TestUnComment()
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
            //dotnet-aop-uncomment System.Console.WriteLine(dt.ToString());
            dt=dt;
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
            System.Console.WriteLine(" + "\"startProgram_Main_6\"" + @");
            var dt = DateTime.Now;
            System.Console.WriteLine(dt.ToString());
            dt=dt;
            System.Console.WriteLine(" + "\"endProgram_Main_6\"" + @");
        }
    }
}";
            result = result.Replace(" ", "").Replace(Environment.NewLine, "");
            newCode = newCode.Replace(" ", "").Replace(Environment.NewLine, "");
            result.ShouldBe(newCode);
        }
    }
}

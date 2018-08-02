using AOPRoslyn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestAOP
{
    [TestClass]
    public class TestRegion
    {
        [TestMethod]
        public void TestOneRegion()
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
#region test
              var dt=DateTime.Now;
#endregion
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
            //was a region 
            var dt = DateTime.Now;
            System.Console.WriteLine(" + "\"endProgram_Main_6\"" + @");
        }
    }
}";
            Assert.AreEqual(result.Replace(Environment.NewLine, "").Replace(" ",""), newCode.Replace(Environment.NewLine, "").Replace(" ", ""));
        }
    }
}

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
            Console.WriteLine(" + "\"Program_Main_6\"" + @");
            //was a region 
            var dt = DateTime.Now;
            
        }
    }
}";
            Assert.AreEqual(result.Replace(Environment.NewLine, "").Replace(" ",""), newCode.Replace(Environment.NewLine, "").Replace(" ", ""));
        }
    }
}

using AOPRoslyn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestAOP
{
    [TestClass]    
    public class TestYield
    {
        [TestMethod]
        public void TestYieldFor()
        {    
            string fileName = @"ClassesForTesting\TestClassYield.cs";
            var text = File.ReadAllText(fileName); 
            var rcf = new RewriteCodeFile(fileName);            
            rcf.Formatter.FormatterFirstLine += "System.Console.WriteLine({arguments});";
            rcf.Rewrite(); 
            string expected = File.ReadAllText(fileName);
            
            Assert.AreNotEqual(text, expected);
            var fromDisk = File.ReadAllText(fileName + ".expected");
            File.WriteAllText(@"E:\test.cs", expected);
            expected.ShouldBe(fromDisk, StringCompareShould.IgnoreCase | StringCompareShould.IgnoreLineEndings);

            //Assert.AreEqual(fromDisk, expected);
        }
    }
}

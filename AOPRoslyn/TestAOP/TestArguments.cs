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
    public class TestArguments
    {
        [TestMethod]
        public void TestArgumentsFor()
        {
            string fileName = @"ClassesForTesting\TestClassPersonWithArguments.cs";
            var text = File.ReadAllText(fileName); 
            var rcf = new RewriteCodeFile(fileName);            
            rcf.rc.Formatter.FormatterFirstLine += "System.Console.WriteLine({arguments});";
            rcf.Rewrite(); 
            string expected = File.ReadAllText(fileName);
            
            Assert.AreNotEqual(text, expected);
            var fromDisk = File.ReadAllText(fileName + ".expected");
            expected.ShouldBe(fromDisk, StringCompareShould.IgnoreCase);

            //Assert.AreEqual(fromDisk, expected);
            FileInfo fi = new FileInfo(fileName);
            fi.IsReadOnly = false;
            File.WriteAllText(fileName, text);
        }
    }
}

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
    public class TestRewriteCode
    {
        [TestMethod]
        public void TestModifierMethod()
        {
            var rc = new RewriteCode();
            rc.Options.PreserveLinesNumber = false;
            rc.Formatter.MethodsToLog = ModifierMethod.PublicKeyword;
            rc.Code = @"
using System;
namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            var dt=DateTime.Now;            
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
            var dt=DateTime.Now;            
            dt=dt;
        }
     }
}";//same code, no aop
            result = result.Replace(" ", "").Replace(Environment.NewLine, "");
            newCode = newCode.Replace(" ", "").Replace(Environment.NewLine, "");
            result.ShouldBe(newCode);
        }


        [TestMethod]
        public void TestAOPFile()
        {
            string fileName = @"ClassesForTesting\TestClassPerson.cs";
            var text = File.ReadAllText(fileName);
            var rc = new RewriteCodeFile(fileName);
            rc.Rewrite();
            string expected = File.ReadAllText(fileName);
            Assert.AreNotEqual(text, expected);
            Assert.AreEqual(File.ReadAllText(fileName + ".expected"), expected);
            FileInfo fi = new FileInfo(fileName);
            fi.IsReadOnly = false;
            File.WriteAllText(fileName, text);
        }
        [TestMethod]
        public void TestAOPFolder()
        {
            string folderName = @"ClassesForTesting";
            int nr = 0;
            var rc = new RewriteCodeFolder(folderName, "*.cs");
            var fileText = new Dictionary<string, string>();
            rc.StartProcessingFile += (object sender, string fileName) =>
            {
                nr++;
                FileInfo fi = new FileInfo(fileName);
                fi.IsReadOnly = false;
                fileText[fileName] = File.ReadAllText(fileName);
            };
            rc.EndProcessingFile += (object sender, string fileName) =>
            {
                File.WriteAllText(fileName, fileText[fileName]);
            };
            rc.Rewrite();
            Assert.IsTrue(nr > 0, "should process at least one file");
        }


    }
}
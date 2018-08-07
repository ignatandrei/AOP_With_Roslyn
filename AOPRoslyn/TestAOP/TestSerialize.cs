using AOPRoslyn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TestAOP
{
    [TestClass]
    public class TestSerialize
    {
        [TestMethod]
        public void TestSerializeRewriteCodeFolder()
        {
            var rc = new RewriteCodeFolder(".", "*.cs");
            var text = rc.SerializeMe();
            var newClass = RewriteAction.UnSerializeMe(text) as RewriteCodeFolder;
            Assert.IsNotNull(newClass);
            //File.WriteAllText("a.txt", text);
            //Process.Start("notepad.exe", "a.txt");
            Assert.AreEqual(rc.FolderName, newClass.FolderName);

        }
        [TestMethod]
        public void TestSerializeRewriteCodeFile()
        {
            var rc = new RewriteCodeFile("andrei.cs");
            var text = rc.SerializeMe();
            var newClass = RewriteAction.UnSerializeMe(text) as RewriteCodeFile;
            Assert.IsNotNull(newClass);
            Assert.AreEqual(rc.FileName, newClass.FileName);

        }
        [TestMethod]
        public void TestSerializeRewriteCodeFolderFormatter()
        {
            var rc = new RewriteCodeFolder(".", "*.cs");
            rc.Formatter.FormatterFirstLine = "System.Console.WriteLine('andrei ignat')";
            var text = rc.SerializeMe();
            var newClass = RewriteAction.UnSerializeMe(text) as RewriteCodeFolder;
            Assert.IsNotNull(newClass);
            //File.WriteAllText("a.txt", text);
            //Process.Start("notepad.exe", "a.txt");
            rc.Formatter.FormatterFirstLine.ShouldBe(newClass.Formatter.FormatterFirstLine);

        }
    }
}

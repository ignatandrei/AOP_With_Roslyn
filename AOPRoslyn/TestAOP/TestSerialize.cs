using AOPRoslyn;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
    }
}

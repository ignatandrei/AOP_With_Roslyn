using AOPRoslyn;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestAOP
{
    [TestClass]
    public class TestAOPExtension
    {
        [TestMethod]
        public void TestExtensionIsModifierOnEnum()
        {
            var typeMM = typeof(ModifierMethod);
            var typeSK = typeof(SyntaxKind);
            foreach (var str in Enum.GetNames(typeMM)){
                var mm = (ModifierMethod)Enum.Parse(typeMM, str);
                if (mm == ModifierMethod.None || mm == ModifierMethod.All)
                    continue;

                var sk = (SyntaxKind)Enum.Parse(typeSK, str);
                var ret = mm.IsOnEnum(sk);
                Assert.IsTrue(ret,$"{mm} == {sk} ");
            }
        }
    }
}

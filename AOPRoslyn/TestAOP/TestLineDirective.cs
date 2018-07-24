using AOPRoslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TestAOP
{
    [TestClass]
    public class TestLineDirective
    {
        [TestMethod]
        public void TestLineSimple()
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
#line 9
            var dt = DateTime.Now;
            Console.WriteLine(" + "\"endProgram_Main_6\"" + @");
        }
    }
}";
            Assert.AreEqual(result.Replace(" ","").Replace(Environment.NewLine, ""), newCode.Replace(" ", "").Replace(Environment.NewLine, ""));
        }

        [TestMethod]
        public void TestException()
        {
            var rc = new RewriteCode(
                new AOPFormatter()
                {
                    FormatterFirstLine = "System.Console.WriteLine(\"start {nameClass}_{nameMethod}_{lineStartNumber}\");",
                    FormatterLastLine = "System.Console.WriteLine(\"end {nameClass}_{nameMethod}_{lineStartNumber}\");"
                }
                );
            rc.Code = @"string s = null; 
// some comment at line 2
var upper = X.Test(s); // Null reference exception at line 3
// more code
class X{
public static string Test(string s) {
    return s.ToUpper();
}
}";

            try
            {
                var result = CSharpScript.EvaluateAsync<int>(rc.Code
                    , ScriptOptions.Default.WithEmitDebugInformation(true)).Result;

            }
            catch (AggregateException e)
            {
                if (e.InnerException is NullReferenceException inner)
                {
                    var startIndex = inner.StackTrace.IndexOf(":line ", StringComparison.Ordinal) + 6;
                    var lineNumberStr = inner.StackTrace.Substring(
                        startIndex, inner.StackTrace.IndexOf("\r", StringComparison.Ordinal) - startIndex);
                    var lineNumber = Int32.Parse(lineNumberStr);

                    Assert.AreEqual(7, lineNumber);
                    
                }
                else
                {
                    Assert.AreEqual(true, false, " should have exception");
                }
            }
            try
            {
                var q = rc.RewriteCodeMethod();
                var result = CSharpScript.EvaluateAsync<int>(q
                    , ScriptOptions.Default.WithEmitDebugInformation(true)).Result;

            }
            catch (AggregateException e)
            {
                if (e.InnerException is NullReferenceException inner)
                {
                    var startIndex = inner.StackTrace.IndexOf(":line ", StringComparison.Ordinal) + 6;
                    var lineNumberStr = inner.StackTrace.Substring(
                        startIndex, inner.StackTrace.IndexOf("\r", StringComparison.Ordinal) - startIndex);
                    var lineNumber = Int32.Parse(lineNumberStr);

                    Assert.AreEqual(7, lineNumber);
                    
                }
                else
                {
                    Assert.AreEqual(true, false, " should have exception");
                }
            }

        }

        [TestMethod]
        public void TestNoLineRefactoring()
        {
            var rc = new RewriteCode(
                new AOPFormatter()
                {
                    FormatterFirstLine = "System.Console.WriteLine(\"start {nameClass}_{nameMethod}_{lineStartNumber}\");",
                    FormatterLastLine = "System.Console.WriteLine(\"end {nameClass}_{nameMethod}_{lineStartNumber}\");"
                }
                );
            rc.Code = @"string s = null; 
// some comment at line 2
var upper = Test(s); // Null reference exception at line 3
// more code
//class X{
public static string Test(string s) {
    return s.ToUpper();
}
//}";

            try
            {
                var result = CSharpScript.EvaluateAsync<int>(rc.Code
                    , ScriptOptions.Default.WithEmitDebugInformation(true)).Result;

            }
            catch (AggregateException e)
            {
                if (e.InnerException is NullReferenceException inner)
                {
                    var startIndex = inner.StackTrace.IndexOf(":line ", StringComparison.Ordinal) + 6;
                    var lineNumberStr = inner.StackTrace.Substring(
                        startIndex, inner.StackTrace.IndexOf("\r", StringComparison.Ordinal) - startIndex);
                    var lineNumber = Int32.Parse(lineNumberStr);

                    Assert.AreEqual(7, lineNumber);

                }
                else
                {
                    Assert.AreEqual(true, false, " should have exception");
                }
            }
            try
            {
                var q = rc.RewriteCodeMethod();
                var result = CSharpScript.EvaluateAsync<int>(q
                    , ScriptOptions.Default.WithEmitDebugInformation(true)).Result;

            }
            catch (AggregateException e)
            {
                if (e.InnerException is NullReferenceException inner)
                {
                    var startIndex = inner.StackTrace.IndexOf(":line ", StringComparison.Ordinal) + 6;
                    var lineNumberStr = inner.StackTrace.Substring(
                        startIndex, inner.StackTrace.IndexOf("\r", StringComparison.Ordinal) - startIndex);
                    var lineNumber = Int32.Parse(lineNumberStr);

                    Assert.AreEqual(7, lineNumber);

                }
                else
                {
                    Assert.AreEqual(true, false, " should have exception");
                }
            }

        }
        static string ReturnStackTraceError(string code)
        {
            var co= new CSharpCompilationOptions(
                outputKind:OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel:OptimizationLevel.Debug);

            var c = CreateCompilationWithMscorlib("orig", code,
                compilerOptions: co);

            var arr = EmitToArray(c);
            var assOrig = Assembly.Load(arr);
            Type t = assOrig.GetType("Test1.Program");
            MethodInfo method = t.GetMethod("Main");
            
            var data= method.Invoke(null, null);
            return data.ToString();
            
            
            
        }

        //https://www.codeproject.com/Articles/1215168/Using-Roslyn-for-Compiling-Code-into-Separate-Net
        private static byte[] EmitToArray(Compilation compilation)
        {
            using (var stream = new MemoryStream())
            {
                // emit result into a stream
                var emitResult = compilation.Emit(stream);

                if (!emitResult.Success)
                {
                    // if not successful, throw an exception
                    Diagnostic firstError =
                        emitResult
                            .Diagnostics
                            .FirstOrDefault
                            (
                                diagnostic =>
                                    diagnostic.Severity == DiagnosticSeverity.Error
                            );

                    throw new Exception(firstError?.GetMessage());
                }

                // get the byte array from a stream
                return stream.ToArray();
            }
        }
        private static CSharpCompilation CreateCompilationWithMscorlib
        (
            string assemblyOrModuleName,
            string code,
            CSharpCompilationOptions compilerOptions = null,
            IEnumerable<MetadataReference> references = null)
        {
            // create the syntax tree
            SyntaxTree syntaxTree = SyntaxFactory.ParseSyntaxTree(code, null, "");

            // get the reference to mscore library
            MetadataReference mscoreLibReference =
                AssemblyMetadata
                    .CreateFromFile(typeof(string).Assembly.Location)
                    .GetReference();

            // create the allReferences collection consisting of 
            // mscore reference and all the references passed to the method
            IEnumerable<MetadataReference> allReferences =
                new MetadataReference[] { mscoreLibReference };
            if (references != null)
            {
                allReferences = allReferences.Concat(references);
            }

            // create and return the compilation
            CSharpCompilation compilation = CSharpCompilation.Create
            (
                assemblyOrModuleName,
                new[] { syntaxTree },
                options: compilerOptions,
                references: allReferences
            );

            return compilation;
        }
    }
}


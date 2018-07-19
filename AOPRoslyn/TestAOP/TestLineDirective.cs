using AOPRoslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
            Console.WriteLine(" + "\"Program_Main_6\"" + @");
#line 9
            var dt = DateTime.Now;
        }
    }
}";
            Assert.AreEqual(result.Replace(Environment.NewLine, ""), newCode.Replace(Environment.NewLine, ""));
        }

        [TestMethod]
        public void TestException()
        {

            var rc=new RewriteCode("string s=\"this is method {nameMethod} from class {nameClass} at line {lineStartNumber}\";");
            rc.Code = @"
using System;
namespace Test1
{
    public class Program
    {
        public static string Main()
        {
              var dt=DateTime.Now;
try{
    throw new ArgumentException(""ersssror"");
}
catch(Exception ex){
    return ex.ToString();
}
    return """";
        }
     }
}";
            var data1 = ReturnStackTraceError(rc.Code);
            var data2= ReturnStackTraceError(rc.RewriteCodeMethod());
            Assert.AreEqual(data1, data2);
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


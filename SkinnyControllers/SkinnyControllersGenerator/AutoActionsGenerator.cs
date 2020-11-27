using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SkinnyControllersGenerator
{


    [Generator]
    public class AutoActionsGenerator : ISourceGenerator
    {
        GeneratorExecutionContext context;
        static Diagnostic DoDiagnostic(DiagnosticSeverity ds,string message)
        {
            //info  could be seen only with 
            // dotnet build -v diag
            var dd = new DiagnosticDescriptor("SkinnyControllersGenerator", $"StartExecution", $"{message}", "SkinnyControllers", ds, true);
            var d = Diagnostic.Create(dd, Location.None,"andrei.txt" );
            return d;
        }
        string autoActions = typeof(AutoActionsAttribute).Name;
        public void Execute(GeneratorExecutionContext context)
        {
            this.context = context;
            string name = $"{ThisAssembly.Project.AssemblyName} {ThisAssembly.Info.Version}";
            context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, name));

            //if (!context.Compilation.ReferencedAssemblyNames.Any(ai => ai.Name.Equals("SkinnyControllersCommon", StringComparison.OrdinalIgnoreCase)))
            //{
            //    var dd= new DiagnosticDescriptor("Andrei","do not have skinny controllers common", "do not have skinny controllers common", "SkinnyControllers", DiagnosticSeverity.Error, true);
            //    var d = Diagnostic.Create(dd, Location.Create("skinnycontrollers.cs", new TextSpan(1,2),new LinePositionSpan()));
            //    context.ReportDiagnostic(d);
            //}

            if (!(context.SyntaxReceiver is SyntaxReceiverFields receiver))
                return;

            context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, "starting data"));
            var compilation = context.Compilation;
            var fieldSymbols = new List<IFieldSymbol>();
            foreach (var field in receiver.CandidateFields)
            {
                SemanticModel model = compilation.GetSemanticModel(field.SyntaxTree);
                foreach (var variable in field.Declaration.Variables)
                {
                    var fieldSymbol = model.GetDeclaredSymbol(variable) as IFieldSymbol;
                    var attr = fieldSymbol.GetAttributes();
                    if (attr.Any(ad => ad.AttributeClass.Name == autoActions))
                    {
                        fieldSymbols.Add(fieldSymbol);
                    }
                }

                var g = fieldSymbols.GroupBy(f => f.ContainingType).ToArray();

                foreach (var group in g)
                {
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, $"starting class {group.Key}"));
                    try
                    {
                        string classSource = ProcessClass(group.Key, group.ToArray(), context);
                        if (string.IsNullOrWhiteSpace(classSource))
                            continue;


                        context.AddSource($"{group.Key.Name}_autogenerate.cs", SourceText.From(classSource, Encoding.UTF8));

                    }
                    catch (Exception ex)
                    {
                        context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"{group.Key} error {ex.Message}"));
                    }
                }
            }
        }

        private string ProcessClass(INamedTypeSymbol classSymbol, IFieldSymbol[] fields, GeneratorExecutionContext context)
        {
            //var d=Diagnostic.Create(new DiagnosticDescriptor())
              //  $"processing {classSymbol.Name}");
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {                
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"class {classSymbol.Name} is in other namespace; please put directly "));
                return null;                 
            }
            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var cd = new ClassDefinition();
            cd.NamespaceName = namespaceName;
            cd.ClassName = classSymbol.Name;
            cd.DictNameField_Methods = fields
                .SelectMany(it => ProcessField(it))
                .GroupBy(it=>it.FieldName)
                .ToDictionary(it => it.Key, it => it.ToArray());
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SkinnyControllersGenerator.AllPost.txt");
            using var reader = new StreamReader(stream);
            var post = reader.ReadToEnd();
            var template = Scriban.Template.Parse(post);
            var output = template.Render(cd, member => member.Name);
            return output;
//            return $@"
//using System.CodeDom.Compiler;
//using System.Runtime.CompilerServices;
///*
//{output}
//*/
//                    namespace {cd.NamespaceName} {{
// [GeneratedCode(""{ThisAssembly.Info.Product}"", ""{ThisAssembly.Info.Version}"")]
//    [CompilerGenerated]
//                        partial class {cd.ClassName}{{ 

//                                private int id(){{
//System.Diagnostics.Debugger.Break();
//return 1;                                    
//}} 
//                            }} 
//                    }}";

        }

        private MethodDefinition[] ProcessField(IFieldSymbol fieldSymbol)
        {
            var ret = new List<MethodDefinition>();
            var code = new StringBuilder();
            string fieldName = fieldSymbol.Name;
            var fieldType = fieldSymbol.Type;
            var members = fieldType.GetMembers();
            foreach(var m in members)
            {
                if (m.IsStatic)
                    continue;
                if (m.IsAbstract)
                    continue;

                if (m.Kind != SymbolKind.Method)
                {
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"{m.Name} is not a method ? "));
                    continue; 

                }

                var ms = m as IMethodSymbol;
                if (ms is null)
                {
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"{m.Name} is not a IMethodSymbol"));
                    continue; 

                }
                if ((ms.Name == fieldName || ms.Name==".ctor") && ms.ReturnsVoid)
                    continue;

                var md = new MethodDefinition();
                md.Name = ms.Name;
                md.FieldName = fieldName;
                md.ReturnsVoid = ms.ReturnsVoid;
                md.ReturnType = ms.ReturnType.ToString();
                md.Parameters = ms.Parameters.ToDictionary(it => it.Name, it => it.Type);

                ret.Add( md);
            }
            return ret.ToArray();
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiverFields());
            //in development
            //Debugger.Launch();
        }
    }
}

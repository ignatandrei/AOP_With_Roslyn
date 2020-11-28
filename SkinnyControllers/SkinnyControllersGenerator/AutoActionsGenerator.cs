using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        static Diagnostic DoDiagnostic(DiagnosticSeverity ds, string message)
        {
            //info  could be seen only with 
            // dotnet build -v diag
            var dd = new DiagnosticDescriptor("SkinnyControllersGenerator", $"StartExecution", $"{message}", "SkinnyControllers", ds, true);
            var d = Diagnostic.Create(dd, Location.None, "andrei.txt");
            return d;
        }
        string autoActions = typeof(AutoActionsAttribute).Name;
        public void Execute(GeneratorExecutionContext context)
        {
            this.context = context;
            string name = $"{ThisAssembly.Project.AssemblyName} {ThisAssembly.Info.Version}";
            context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, name));

            if (!(context.SyntaxReceiver is SyntaxReceiverFields receiver))
                return;

            context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, "starting data"));
            var compilation = context.Compilation;
            var fieldSymbols = new List<IFieldSymbol>();
            foreach (var classDec in receiver.CandidatesControllers)
            {
                SemanticModel model = compilation.GetSemanticModel(classDec.SyntaxTree);
                var attrArray = classDec.AttributeLists;
                var myController = model.GetDeclaredSymbol(classDec);
                var att = myController.GetAttributes()
                    .FirstOrDefault(it => it.AttributeClass.Name == autoActions);
                if (att == null)
                    continue;

                //verify for null
                var template = att.NamedArguments.First(it => it.Key == "template")
                    .Value
                    .Value
                    .ToString();
                var templateId = (TemplateIndicator)long.Parse(template);
                var fields = att.NamedArguments.First(it => it.Key == "FieldsName")
                    .Value
                    .Values
                    .Select(it => it.Value)
                    .ToArray()
                    ;
                var memberFields = myController
                        .GetMembers()
                        .Where(it => fields.Contains(it.Name))
                        .Select(it => it as IFieldSymbol)
                        .Where(it => it != null)
                        .ToArray();
                if (memberFields.Length < fields.Length)
                {
                    //report also the mismatched names
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning,
                                $"controller {myController.Name} have some fields to generate that were not found"));
                }
                if (memberFields.Length == 0)
                {
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning,
                            $"controller {myController.Name} do not have fields to generate"));
                }
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, $"starting class {myController.Name}"));
                try
                {
                    string classSource = ProcessClass(myController, memberFields, templateId);
                    if (string.IsNullOrWhiteSpace(classSource))
                        continue;


                    context.AddSource($"{myController.Name}.autogenerate.cs", SourceText.From(classSource, Encoding.UTF8));

                }
                catch (Exception ex)
                {
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Error, $"{myController.Name} error {ex.Message}"));
                }

            }
        }

        private string ProcessClass(INamedTypeSymbol classSymbol, IFieldSymbol[] fields, TemplateIndicator ti)
        {
            if (ti == TemplateIndicator.None)
            {
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, $"class {classSymbol.Name} has no template "));
                return null;
            }
            
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
                .GroupBy(it => it.FieldName)
                .ToDictionary(it => it.Key, it => it.ToArray());




            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SkinnyControllersGenerator.AllPost.txt");
            using var reader = new StreamReader(stream);
            var post = reader.ReadToEnd();
            var template = Scriban.Template.Parse(post);
            var output = template.Render(cd, member => member.Name);
            return output;
            
        }

        private MethodDefinition[] ProcessField(IFieldSymbol fieldSymbol)
        {
            var ret = new List<MethodDefinition>();
            var code = new StringBuilder();
            string fieldName = fieldSymbol.Name;
            var fieldType = fieldSymbol.Type;
            var members = fieldType.GetMembers();
            foreach (var m in members)
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
                if ((ms.Name == fieldName || ms.Name == ".ctor") && ms.ReturnsVoid)
                    continue;

                var md = new MethodDefinition();
                md.Name = ms.Name;
                md.FieldName = fieldName;
                md.ReturnsVoid = ms.ReturnsVoid;
                md.ReturnType = ms.ReturnType.ToString();
                md.Parameters = ms.Parameters.ToDictionary(it => it.Name, it => it.Type);

                ret.Add(md);
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
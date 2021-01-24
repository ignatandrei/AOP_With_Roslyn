using AOPEFCommon;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AOPEFGenerator
{


    [Generator]
    public partial class AutoEFGenerator : ISourceGenerator
    {
        Assembly executing;
        GeneratorExecutionContext context;
        static Diagnostic DoDiagnostic(DiagnosticSeverity ds, string message)
        {
            //info  could be seen only with 
            // dotnet build -v diag
            var dd = new DiagnosticDescriptor("AOPEF", $"StartExecution", $"{message}", "SkinnyControllers", ds, true);
            var d = Diagnostic.Create(dd, Location.None, "andrei.txt");
            return d;
        }
        string autoMethods = typeof(TemplateAttribute).Name;
        public void Execute(GeneratorExecutionContext context)
        {
            this.context = context;
            string name = $"{ThisAssembly.Project.AssemblyName} {ThisAssembly.Info.Version}";
            context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, name));
            this.executing = Assembly.GetExecutingAssembly();
            if (!(context.SyntaxReceiver is SyntaxReceiverClass receiver))
                return;

            context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, "starting data"));
            GenerateRepository(context, receiver);
        }
        public void GenerateRepository(GeneratorExecutionContext context, SyntaxReceiverClass receiver)
        {

            if ((receiver.CandidatesClasses?.Count ?? 0) == 0)
                return;

            var compilation = context.Compilation;
            var fieldSymbols = new List<IFieldSymbol>();
            foreach (var classDec in receiver.CandidatesClasses)
            {
                var model = compilation.GetSemanticModel(classDec.SyntaxTree);
                var attrArray = classDec.AttributeLists;
                var classWithMethods = model.GetDeclaredSymbol(classDec);
                
                var attAll = classWithMethods.GetAttributes()
                    .Where(it => it.AttributeClass.Name == autoMethods).ToArray();

                
                if (attAll.Length == 0)
                    continue;
                for (int i = 0; i < attAll.Length; i++)
                {

                    var att = attAll[i];
                    //verify for null
                    var template = att.NamedArguments.FirstOrDefault(it => it.Key == "template")
                        .Value
                        .Value
                        ?.ToString();
                    if (string.IsNullOrEmpty(template))
                    {
                        context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning,
                                    $"class {classWithMethods.Name} do not have a template for {nameof(TemplateAttribute)}. At least put [AutoMethods(template = TemplateMethod.None)]"));
                        continue;
                    }

                    var templateId = (TemplateMethod)long.Parse(template);
                    //var pocoName = att.NamedArguments.FirstOrDefault(it => it.Key == "POCOName")
                    //    .Value
                    //    .Value
                    //    ?.ToString();
                    //;

                    var pk1 = att.NamedArguments.FirstOrDefault(it => it.Key == "PK1")
                        .Value
                        .Value
                        ?.ToString();
                    ;

                    var pk2 = att.NamedArguments.FirstOrDefault(it => it.Key == "PK2")
                        .Value
                        .Value
                        ?.ToString();

                    //var poco = context.Compilation.GetSymbolsWithName(pocoName).FirstOrDefault();


                    string templateCustom = "";
                    if (att.NamedArguments.Any(it => it.Key == "CustomTemplateFileName"))
                    {

                        templateCustom = att.NamedArguments.First(it => it.Key == "CustomTemplateFileName")
                        .Value
                        .Value
                        .ToString()
                        ;
                    }

                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, $"starting class {classWithMethods.Name} with template {templateId}"));
                    string post = "";
                    try
                    {
                        switch (templateId)
                        {

                            case TemplateMethod.None:
                                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, $"class {classWithMethods.Name} has no template "));
                                continue;
                            case TemplateMethod.CustomTemplateFile:


                                var file = context.AdditionalFiles.FirstOrDefault(it => it.Path.EndsWith(templateCustom));
                                if (file == null)
                                {
                                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Error, $"cannot find {templateCustom} for  {classWithMethods.Name} . Did you put in AdditionalFiles in csproj ?"));
                                    continue;
                                }
                                post = file.GetText().ToString();
                                break;

                            default:

                                using (var stream = executing.GetManifestResourceStream($"AOPEFGenerator.templates.{templateId}.txt"))
                                {
                                    using var reader = new StreamReader(stream);
                                    post = reader.ReadToEnd();

                                }
                                break;
                        }


                        string classSource = ProcessClass(classWithMethods, pk1, pk2, post);
                        if (string.IsNullOrWhiteSpace(classSource))
                            continue;


                        context.AddSource($"{classWithMethods.Name}_{i}.autogenerate.cs", SourceText.From(classSource, Encoding.UTF8));

                    }
                    catch (Exception ex)
                    {
                        context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Error, $"{classWithMethods.Name} error {ex.Message}"));
                    }
                }

            }
        }

        private string ProcessClass(INamedTypeSymbol classSymbol,  string PK1, string PK2, string post)
        {


            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"class {classSymbol.Name} is in other namespace; please put directly "));
                return null;
            }
            var interfaces = classSymbol.Interfaces;
            
            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var cd = new ClassRepositoryDefinition();
            //cd.POCOName = pocoName;
            foreach (var item in interfaces)
            {
                if (!item.IsGenericType)
                    continue;

                //very particular IRepository<dboDepartment,Int64>
                var ta = item.TypeArguments;
                var argsLength = ta.Length;
                var poco = ta.First();
                cd.POCOFullName = poco.Name;
                cd.POCOName = poco.Name;
                if (!string.IsNullOrWhiteSpace(poco.ContainingNamespace?.Name))
                    cd.POCOFullName = poco.ContainingNamespace.Name+ "." + poco.Name;
                //work here for 0 or 2 PK
                if(!string.IsNullOrWhiteSpace(PK1) && argsLength>1)
                {
                    var PKType = ta[1];
                    cd.PK1Type = PKType.Name;
                    if (!string.IsNullOrWhiteSpace(PK2) && argsLength>2)
                    {
                        PKType = ta[2];
                        cd.PK2Type = PKType.Name;

                    }

                }
            }
            //cd.POCOFullName = pocoName;
            //if (pocoName.Contains("."))
            //{
            //    cd.POCOName = cd.POCOName.Split('.').Last();
            //}
            cd.PK1 = PK1;
            cd.PK2 = PK2;
            
            cd.Original = classSymbol;
            cd.NamespaceName = namespaceName;
            cd.ClassName = classSymbol.Name;
            var m = FindMethods(classSymbol);
            cd.Methods = m;
            var p = FindProperties(classSymbol);
            cd.Properties = p;
            var template = Scriban.Template.Parse(post);
            var output = template.Render(cd, member => member.Name);
            return output;

        }
        static MethodKind[] allowed = new[] {
            MethodKind.ExplicitInterfaceImplementation,
            MethodKind.Ordinary

        };
        private PropertyDefinition[] FindProperties(INamedTypeSymbol fieldSymbol)
        {
            var ret = new List<PropertyDefinition>();
            var code = new StringBuilder();
            string fieldName = fieldSymbol.Name;
            var members = fieldSymbol.GetMembers().OfType<IPropertySymbol>().ToArray();
            foreach (var m in members)
            {
                if (m.IsStatic)
                    continue;

                if (m.Kind != SymbolKind.Property)
                {
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"{m.Name} is not a method ? "));
                    continue;

                }

                var ms = m as IPropertySymbol;
                if (ms is null)
                {
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"{m.Name} is not a IMethodSymbol"));
                    continue;

                }


                PropertyDefinition pd = new();
                pd.Original = ms;
                pd.Name = ms.Name;
                pd.ReturnType = ms.Type.ToString();
                pd.Accesibility = (int)ms.DeclaredAccessibility;
                ret.Add(pd);
            }
            if (ret.Count == 0)
            {
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning,
                    $"could not find methods on {fieldName} from {fieldSymbol.ContainingType?.Name}"));
            }
            return ret.ToArray();
        }

        private MethodDefinition[] FindMethods(INamedTypeSymbol fieldSymbol)
        {
            var ret = new List<MethodDefinition>();
            var code = new StringBuilder();
            string fieldName = fieldSymbol.Name;
            var members = fieldSymbol.GetMembers().OfType<IMethodSymbol>();
            foreach (var m in members)
            {
                if (m.IsStatic)
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
                if (ms.DeclaredAccessibility == Accessibility.Public)
                    continue;

                if (ms.MethodKind != MethodKind.Ordinary)
                    continue;

                if ((ms.Name == fieldName || ms.Name == ".ctor") && ms.ReturnsVoid)
                    continue;

                MethodDefinition md = new();
                md.Original = ms;
                md.Name = ms.Name;
                md.ReturnsVoid = ms.ReturnsVoid;
                md.ReturnType = ms.ReturnType.ToString();
                md.Parameters = ms.Parameters.ToDictionary(it => it.Name, it => it.Type);
                ret.Add(md);
            }
            if (ret.Count == 0)
            {
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning,
                    $"could not find methods on {fieldName} from {fieldSymbol.ContainingType?.Name}"));
            }
            return ret.ToArray();
        }


    }
}
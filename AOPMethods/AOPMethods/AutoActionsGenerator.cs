﻿using AOPMethodsCommon;
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

namespace AOPMethodsGenerator
{


    [Generator]
    public partial class AutoActionsGenerator : ISourceGenerator
    {
        Assembly executing;
        GeneratorExecutionContext context;
        static Diagnostic DoDiagnostic(DiagnosticSeverity ds, string message)
        {
            //info  could be seen only with 
            // dotnet build -v diag
            var dd = new DiagnosticDescriptor("AOPMethods", $"StartExecution", $"{message}", "SkinnyControllers", ds, true);
            var d = Diagnostic.Create(dd, Location.None, "andrei.txt");
            return d;
        }
        string autoMethods = typeof(AutoMethodsAttribute).Name;
        public void Execute(GeneratorExecutionContext context)
        {
            this.context = context;
            
            string name = $"{ThisAssembly.Project.AssemblyName} {ThisAssembly.Info.Version}";
            context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, name));

            if (!(context.SyntaxReceiver is SyntaxReceiverClass receiver))
                return;

            context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, "starting data"));

            if ((receiver.CandidatesClasses?.Count ?? 0) == 0)
                return;

            this.executing = Assembly.GetExecutingAssembly();
            var compilation = context.Compilation;
            var fieldSymbols = new List<IFieldSymbol>();
            foreach (var classDec in receiver.CandidatesClasses)
            {
                var model = compilation.GetSemanticModel(classDec.SyntaxTree);
                var attrArray = classDec.AttributeLists;
                var myController = model.GetDeclaredSymbol(classDec);
                var att = myController.GetAttributes()
                    .FirstOrDefault(it => it.AttributeClass.Name == autoMethods);
                if (att == null)
                    continue;

                //verify for null
                var template = att.NamedArguments.FirstOrDefault(it => it.Key == "template")
                    .Value
                    .Value
                    ?.ToString();
                if (string.IsNullOrEmpty(template))
                {
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning,
                                $"class {myController.Name} do not have a template for {nameof(AutoMethodsAttribute)}. At least put [AutoMethods(template = TemplateMethod.None)]"));
                    continue;
                }
            
                var templateId = (TemplateMethod)long.Parse(template);
                var fields = att.NamedArguments.FirstOrDefault(it => it.Key == "FieldsName")
                    .Value
                    .Values
                    .Select(it => it.Value?.ToString())
                    ?.ToArray()
                    ;
                string[] excludeFields = null;
                //try
                //{
                //    excludeFields = att.NamedArguments.FirstOrDefault(it => it.Key == "ExcludeFields")
                //        .Value
                //        .Values
                //        .Select(it => it.Value?.ToString())
                //        .ToArray()
                //        ;
                //}
                //catch (Exception)
                //{
                //    //it is not mandatory to define ExcludeFields
                //    //do nothing, 
                //}
                string templateCustom = "";
                if (att.NamedArguments.Any(it => it.Key == "CustomTemplateFileName"))
                {

                    templateCustom = att.NamedArguments.First(it => it.Key == "CustomTemplateFileName")
                    .Value
                    .Value                    
                    .ToString()
                    ;
                }
                bool All = fields.Contains("*");

                var memberFields = myController
                        .GetMembers()
                        .Where(it => All || fields.Contains(it.Name))
                        .Select(it => it as IFieldSymbol)
                        .Where(it => it != null)                        
                        .ToArray();

                if (excludeFields?.Length > 0)
                {
                    var q = memberFields.ToList();
                    q.RemoveAll(it => excludeFields.Contains(it.Name));
                    memberFields = q.ToArray();

                }
                if (memberFields.Length < fields.Length)
                {
                    //report also the mismatched names
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning,
                                $"controller {myController.Name} have some fields to generate that were not found"));
                }
                if (memberFields.Length == 0)
                {
                    context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Error,
                            $"controller {myController.Name} do not have fields to generate"));
                    continue;
                }
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, $"starting class {myController.Name} with template {templateId}"));
                string post = "";
                try
                {
                    switch (templateId)
                    {

                        case TemplateMethod.None:
                            context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Info, $"class {myController.Name} has no template "));
                            continue;
                        case TemplateMethod.CustomTemplateFile:

                            var file = context.AdditionalFiles.FirstOrDefault(it => it.Path.EndsWith(templateCustom));
                            if (file == null)
                            {
                                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Error, $"cannot find {templateCustom} for  {myController.Name} . Did you put in AdditionalFiles in csproj ?"));
                                continue;
                            }
                            post = file.GetText().ToString();
                            break;

                        default:
                            using (var stream = executing.GetManifestResourceStream($"AOPMethodsGenerator.templates.{templateId}.txt"))
                            {
                                using var reader = new StreamReader(stream);
                                post = reader.ReadToEnd();

                            }
                            break;
                    }

                    string classSource = ProcessClass(myController, memberFields, post);
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

        private string ProcessClass(INamedTypeSymbol classSymbol, IFieldSymbol[] fields, string post)
        {
           

            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"class {classSymbol.Name} is in other namespace; please put directly "));
                return null;
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var cd = new ClassDefinition();
            cd.NamespaceName = namespaceName;
            cd.ClassName = classSymbol.Name;
            if (fields.Length== 0)
            {
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"class {cd.ClassName} has {fields.Length} fields to process"));
            }
            cd.DictNameField_Methods = fields
                .SelectMany(it => ProcessField(it))
                .GroupBy(it => it.FieldName)
                .ToDictionary(it => it.Key, it => it.ToArray());


            if (cd.DictNameField_Methods.Count == 0)
            {
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Warning, $"class {cd.ClassName} has 0 fields to process"));
            }
            var template = Scriban.Template.Parse(post);
            var output = template.Render(cd, member => member.Name);
            return output;
            
        }
        static MethodKind[] allowed = new[] {
            MethodKind.ExplicitInterfaceImplementation,
            MethodKind.Ordinary

        };
        private MethodDefinition[] ProcessField(IFieldSymbol fieldSymbol)
        {
           
            var ret = new List<MethodDefinition>();
            var code = new StringBuilder();
            string fieldName = fieldSymbol.Name;
            var fieldType = fieldSymbol.Type;
            var members = fieldType.GetMembers().OfType<IMethodSymbol>();


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
                if (ms.DeclaredAccessibility != Accessibility.Public)
                    continue;
                if (ms.MethodKind != MethodKind.Ordinary)
                    continue;

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
            if (ret.Count== 0)
            {
                context.ReportDiagnostic( DoDiagnostic(DiagnosticSeverity.Warning,
                    $"could not find methods on {fieldName} from {fieldSymbol.ContainingType?.Name}"));
            }
            return ret.ToArray();
        }

        
    }
}
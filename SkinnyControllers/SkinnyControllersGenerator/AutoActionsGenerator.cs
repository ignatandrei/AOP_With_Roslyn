using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SkinnyControllersGenerator
{
    
    [Generator]
    public class AutoActionsGenerator : ISourceGenerator
    {
        string autoActions = typeof(AutoActionsAttribute).Name;
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is SyntaxReceiverFields receiver))
                return;
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

                foreach (var group in fieldSymbols.GroupBy(f => f.ContainingType))
                {
                    string classSource = ProcessClass(group.Key, group.ToArray(),   context);
                    if (string.IsNullOrWhiteSpace(classSource))
                        continue;

                    context.AddSource($"{group.Key.Name}_autogenerate.cs", SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }

        private string ProcessClass(INamedTypeSymbol classSymbol, IFieldSymbol[] fields, GeneratorExecutionContext context)
        {
            //var d=Diagnostic.Create(new DiagnosticDescriptor())
              //  $"processing {classSymbol.Name}");
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
            {
                //log diagnostic
                return null;                 
            }
            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var source = new StringBuilder($@"
namespace {namespaceName}
{{
    public partial class {classSymbol.Name} 
    {{
            public string id;");

            foreach (var fieldSymbol in fields)
            {
                source.AppendLine(ProcessField(fieldSymbol));
            }

            source.Append(@"
    }//end class
}//end namespace
");
            return source.ToString();
        }

        private string ProcessField(IFieldSymbol fieldSymbol)
        {
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
                    continue; //diagnostic
                
                var ms = m as IMethodSymbol;
                if (ms is null)//log diagnostic
                    continue;
                if ((ms.Name == fieldName || ms.Name==".ctor") && ms.ReturnsVoid)
                    continue;

                var parametersDefinition = string.Join(",", ms.Parameters.Select(it => it.Type.ContainingNamespace + "." + it.Type.Name + " " + it.Name).ToArray()); 
                var parametersCall = string.Join(",", ms.Parameters.Select(it => it.Name).ToArray());
                string method = "[Microsoft.AspNetCore.Mvc.HttpGetAttribute]";
                if(ms.Parameters.Any())
                    method = "[Microsoft.AspNetCore.Mvc.HttpPostAttribute]";
                code.AppendLine(method);
                code.AppendLine( $"public {ms.ReturnType} {ms.Name}  ({parametersDefinition})  {{");
                code.AppendLine(ms.ReturnsVoid ? "" : "return ");
                code.AppendLine($"{fieldName}.{ms.Name}({parametersCall});");
                code.AppendLine("}");
            }
            return code.ToString();
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiverFields());

            Debugger.Launch();
        }
    }
}

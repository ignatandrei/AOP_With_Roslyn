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

                foreach (IGrouping<INamedTypeSymbol, IFieldSymbol> group in fieldSymbols.GroupBy(f => f.ContainingType))
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
                source.Append(ProcessField(fieldSymbol));
            }

            source.Append(@"
    }//end class
}//end namespace
");
            return source.ToString();
        }

        private string ProcessField(IFieldSymbol fieldSymbol)
        {
            string fieldName = fieldSymbol.Name;
            var fieldType = fieldSymbol.Type;
            var members = fieldType.GetMembers();
            foreach(var m in members)
            {
                if (m.Kind != SymbolKind.Method)
                    continue;
                if (m.IsStatic)
                    continue;
                if (m.IsAbstract)
                    continue;

                var Q = m.IsStatic;
            }
            return "";
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiverFields());

            Debugger.Launch();
        }
    }
}

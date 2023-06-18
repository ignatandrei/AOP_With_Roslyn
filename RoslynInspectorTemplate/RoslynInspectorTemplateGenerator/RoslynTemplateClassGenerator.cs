

using System.Collections.Immutable;

namespace RoslynInspectorTemplateGenerator;
[Generator]
public class RoslynTemplateClassGenerator : IIncrementalGenerator
{
    const string autoClass = "RoslynTemplateClass";//typeof(AutoActionsAttribute).Name;
    const string autoClassFullName = "RoslynInspectorTemplateCommon.RoslynTemplateClassAttribute";//typeof(AutoActionsAttribute).FullName;
    const string endFileName = ".razor";
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        Initialize1(context);
    }
    void Initialize1(IncrementalGeneratorInitializationContext context)
    {
        var classTypes = context.SyntaxProvider
                             .ForAttributeWithMetadataName(autoClassFullName,
                                                           FindClassWithAttr.ItIsOnClass,
                                                           FindClassWithAttr.GetClassInfo)
                             .Where(it => it.IsValid())
                             .Collect()
                               ;

        var templates = context.AdditionalTextsProvider
                                .Where(text => text.Path.EndsWith(endFileName, StringComparison.OrdinalIgnoreCase))
                                .Select((text, token) => new AdditionalFilesText(text.Path, text.GetText(token)?.ToString()))
                                .Where(text => text.IsValid())
                                .Collect();
        context.RegisterSourceOutput(classTypes.Combine(templates), GenerateCode);

    }
    private void GenerateCode(SourceProductionContext arg1, (ImmutableArray<DataGenerator> Left, ImmutableArray<AdditionalFilesText> Right) arg2)
    {
        var dg = arg2.Left.Distinct().ToArray();
        var files = arg2.Right;
        GenerateCode1(arg1, dg, files);
    }

    private void GenerateCode1(SourceProductionContext context, DataGenerator[] arg2, ImmutableArray<AdditionalFilesText> files)
    {
        foreach (var item in arg2)
        {
            
            var cds = item.Cds;
            
            if (!FindClassWithAttr.IsPartial(cds))
            {
                context.ReportDiagnostic(DoDiagnostic(DiagnosticSeverity.Error, "please add partial declaration to class "+ item.Nts.Name, cds.GetLocation()));
                continue;
            }
            var myClass = item.Nts;
            string? Namespace = myClass.ContainingNamespace.IsGlobalNamespace ? null : myClass.ContainingNamespace.ToString();
            var atts = myClass
                    .GetAttributes()
                    .Where(it => it.AttributeClass.Name.StartsWith(autoClass))
                    .ToArray()
                    ;
            foreach (var att in atts)
            {

                var namedArgs = att.NamedArguments;
                var temp = namedArgs.FirstOrDefault(it => it.Key == "InterceptDataOnClass");
                if (temp.Value.Value == null)
                    continue;
                var template = temp
                        .Value
                        .Value
                        .ToString();
                var intercept = (InterceptDataOnClass)long.Parse(template);

            }
        }
    }
    static Diagnostic DoDiagnostic(DiagnosticSeverity ds, string message, Location? l=null)
    {
        //info  could be seen only with 
        // dotnet build -v diag
        var loc=l ?? Location.None;
        var dd = new DiagnosticDescriptor("RoslynTemplate", $"StartExecution", $"{message}", "SkinnyControllers", ds, true);
        var d = Diagnostic.Create(dd, loc, "andrei.txt");
        return d;
    }

   
}
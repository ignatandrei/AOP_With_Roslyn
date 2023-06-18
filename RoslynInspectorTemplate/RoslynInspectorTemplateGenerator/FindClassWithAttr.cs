
namespace RoslynInspectorTemplateGenerator;

internal class FindClassWithAttr
{
    internal static bool ItIsOnClass(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
            return false;
        //if (!IsPartial(classDeclaration))
        //    return false;
        if (classDeclaration.AttributeLists.Count == 0) return false;

        return true;
    }
    internal static bool IsPartial(ClassDeclarationSyntax classDeclaration)
    {
        return classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
    }

    internal static DataGenerator GetClassInfo(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        var type = context.TargetSymbol as INamedTypeSymbol;
        if (type == null)
            return null;
        var cds = context.TargetNode as ClassDeclarationSyntax;
        if (cds == null)
            return null;

        return new DataGenerator(type, cds);
    }
}

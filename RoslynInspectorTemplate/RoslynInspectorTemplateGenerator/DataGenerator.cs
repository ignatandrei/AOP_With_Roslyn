namespace RoslynInspectorTemplateGenerator;
class DataGenerator : IEquatable<DataGenerator>
{
    public DataGenerator(INamedTypeSymbol nts, ClassDeclarationSyntax cds)
    {
        Nts = nts;
        Cds = cds;
    }

    public INamedTypeSymbol Nts { get; }
    public ClassDeclarationSyntax Cds { get; }

    public bool Equals(DataGenerator other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return Nts?.Name == other.Nts?.Name;
    }
    public bool IsValid()
    {
        return Nts != null && Cds != null;
    }
}



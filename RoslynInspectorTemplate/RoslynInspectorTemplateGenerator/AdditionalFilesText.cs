namespace RoslynInspectorTemplateGenerator;

class AdditionalFilesText
{
    public AdditionalFilesText(string Name, string Contents)
    {
        this.Name = Name;
        this.Contents = Contents;
    }

    public string Name { get; }
    public string Contents { get; }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Contents);
    }
}



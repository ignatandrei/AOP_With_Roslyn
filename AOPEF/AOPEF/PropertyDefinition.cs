using Microsoft.CodeAnalysis;

namespace AOPEFGenerator
{
    class PropertyDefinition
    {
        public IPropertySymbol Original;
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public int Accesibility { get; set; }
    }
}

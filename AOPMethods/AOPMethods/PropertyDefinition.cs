using Microsoft.CodeAnalysis;

namespace AOPMethodsGenerator
{
    class PropertyDefinition
    {
        public IPropertySymbol Original;
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public Accessibility Accesibility { get; set; }
    }
}

using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkinnyControllersGenerator
{
    class ClassDefinition
    {
        public string ClassName;
        public string NamespaceName;
        public Dictionary<string,MethodDefinition[]> DictNameField_Methods;
        public string version = ThisAssembly.Info.Version;    
    }
    class MethodDefinition
    {
        public string Name { get; set; }
        public string FieldName { get; set; }
        public string ReturnType;
        public bool ReturnsVoid;
        //name, type
        public Dictionary<string, ITypeSymbol> Parameters;

        public string parametersDefinitionCSharp => string.Join(",", Parameters.Select(it => it.Value.ContainingNamespace + "." + it.Value.Name + " " + it.Key).ToArray());
        public string parametersCallCSharp => string.Join(",", Parameters.Select(it => it.Key).ToArray());

        public int NrParameters
        {
            get
            {
                return Parameters?.Count ?? 0;
            }
        }

    }
}

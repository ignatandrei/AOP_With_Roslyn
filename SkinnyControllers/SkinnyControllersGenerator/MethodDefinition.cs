using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkinnyControllersGenerator
{
    class MethodDefinition
    {
        public string Name { get; set; }
        public string FieldName { get; set; }
        public string ReturnType;
        public bool ReturnsVoid;
        //name, type
        public Dictionary<string, ITypeSymbol> Parameters;

        public KeyValuePair<string, ITypeSymbol> FirstParameter
        {
            get
            {
                if(Parameters?.Count() > 0)
                {
                    return Parameters.First();
                }
                return default;
            }
        }
        public KeyValuePair<string, ITypeSymbol>? LastParameter
        {
            get
            {
                if (Parameters?.Count() > 0)
                {
                    return Parameters.Last();
                }
                return default;
            }
        }
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

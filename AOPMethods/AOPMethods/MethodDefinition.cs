using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOPMethodsGenerator
{
    class MethodDefinition
    {
        public string NewName { get; set; }
        public string Name { get; set; }
        
        public string ReturnType;
        public bool ReturnsVoid;
        //name, type
        public Dictionary<string, ITypeSymbol> Parameters;
        public IMethodSymbol Original;

        public bool IsAsync { get; set; }
        
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
        public int HashCodeParams => Math.Abs( parametersDefinitionCSharp.GetHashCode());
        public string parametersDefinitionCSharp => string.Join(",", Parameters.Select(it =>  it.Value.ToDisplayString() + " " + it.Key).ToArray());
        public string parametersCallCSharp => string.Join(",", Parameters.Select(it => it.Key).ToArray());

        public string parametersCallWithRecord
        {
            get
            {
                if (NrParameters == 0)
                    return "";
                return "data." + string.Join(",data.", Parameters.Select(it => it.Key).ToArray());
            }
        }
        public int NrParameters
        {
            get
            {
                return Parameters?.Count ?? 0;
            }
        }

    }
}

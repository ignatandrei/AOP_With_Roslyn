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
        public Microsoft.CodeAnalysis.Accessibility DeclaredAccessibility { get; set; }
        public string Accessibility { get; set; }
        public string ReturnType { get; set; }
        public bool ReturnsVoid { get; set; }
        //name, type
        public Dictionary<string, IParameterSymbol> Parameters { get; set; }
        public IMethodSymbol Original { get; set; }

        public bool IsAsync { get; set; }
        public bool CouldUseAsync { get; set; }
        public bool CouldReturnVoidFromAsync { get; set; }
        public KeyValuePair<string, IParameterSymbol> FirstParameter
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
        public KeyValuePair<string, IParameterSymbol>? LastParameter
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
        public string parametersCallCSharp => string.Join(",", Parameters.Select(it =>

        {
            if (!it.Value.HasExplicitDefaultValue)
                return it.Key;

            return $"{it.Key}= {it.Value.ExplicitDefaultValue}"; 
        }
        ).ToArray());

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

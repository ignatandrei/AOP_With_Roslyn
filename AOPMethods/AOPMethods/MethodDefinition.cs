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

        public dynamic this[string parameterName]
        {
            get {
                var param = this.Parameters[parameterName];
                return new { param.RefKind, IsValueType = param.Type.IsValueType,name=param.Type.Name  };
            }
        }
        public KeyValuePair<string, IParameterSymbol>[] ParametersToBeSerialized
        {
            get
            {
                return this.Parameters
                    .Where(it => 
                    (it.Value?.Type?.IsValueType ?? false)
                    ||
                    it.Value?.Type?.Name?.ToLower() == "string"
                    )

                    .ToArray();
            }
        }

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
        public string parametersDefinitionCSharp => string.Join(",", Parameters.Select(it =>
        {
            var str = it.Value.ToDisplayString() + " " + it.Key;
            if (!it.Value.HasExplicitDefaultValue)
                return str;

            return $"{str}= {it.Value.ExplicitDefaultValue?.ToString() ?? "null"}";

        })
        .ToArray());
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

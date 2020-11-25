using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace SkinnyControllersGenerator
{
    class EqComparer : IEqualityComparer<INamedTypeSymbol>
    {
        public bool Equals(INamedTypeSymbol x, INamedTypeSymbol y)
        {
            return SymbolEqualityComparer.IncludeNullability.Equals(x, y);
        }

        public int GetHashCode(INamedTypeSymbol obj)
        {
            return SymbolEqualityComparer.IncludeNullability.GetHashCode(obj);
        }
    }
}

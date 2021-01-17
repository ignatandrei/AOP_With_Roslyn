using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace AOPEFGenerator
{
    class ClassRepositoryDefinition
    {
        public string ClassName;
        public string NamespaceName;
        public string version = ThisAssembly.Info.Version;
        public string POCOName;
        public string POCOFullName;
        public INamedTypeSymbol Original;
        public string PK1;
        public string PK1Type;


    }
}

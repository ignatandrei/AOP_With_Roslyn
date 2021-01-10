using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AOPMethodsGenerator
{
    class EnumDefinition
    {
        public EnumDefinition(int nr)
        {
            Values = new KeyValuePair<long, string>[nr];
        }
        public string NamespaceName;
        public string FullName;
        public string Name;
        public KeyValuePair<long, string>[] Values;
        public string Type;//int or long
        public EnumDeclarationSyntax Original;
        public string Version;
    }
}

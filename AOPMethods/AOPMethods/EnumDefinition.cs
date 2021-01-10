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
            Values = new KeyValuePair<string, long>[nr];
        }
        public string NamespaceName;
        public string FullName;
        public string Name;
        public KeyValuePair<string, long>[] Values;
        public string Type;//int or long
        public EnumDeclarationSyntax Original;
        public string Version;
    }
}

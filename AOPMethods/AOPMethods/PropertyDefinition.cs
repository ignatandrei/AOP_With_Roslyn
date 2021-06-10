﻿using Microsoft.CodeAnalysis;

namespace AOPMethodsGenerator
{
    class PropertyDefinition
    {
        public IPropertySymbol Original { get; set; }
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public int Accesibility { get; set; }
    }
}

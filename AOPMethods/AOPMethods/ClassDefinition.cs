﻿using System.Collections.Generic;

namespace AOPMethodsGenerator
{
    class ClassDefinition
    {
        public string ClassName;
        public string NamespaceName;
        public MethodDefinition[] Methods;
        public string version = ThisAssembly.Info.Version;    
    }
}
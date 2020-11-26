using System;
using System.Collections.Generic;

namespace SkinnyControllersGenerator
{
    class ClassDefinition
    {
        public string ClassName;
        public string NamespaceName;
        public Dictionary<string,MethodDefinition[]> DictNameField_Methods;
        public string version = ThisAssembly.Info.Version;    
    }
    class MethodDefinition
    {
        public string Name { get; set; }
        public string FieldName { get; set; }
        public string ReturnType;
        public bool ReturnsVoid;
        //name, type
        public Dictionary<string, string> Parameters;

    }
}

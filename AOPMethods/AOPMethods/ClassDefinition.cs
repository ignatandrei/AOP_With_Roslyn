using System.Collections.Generic;

namespace AOPMethodsGenerator
{
    class ClassDefinition
    {
        public string ClassName;
        public string NamespaceName;
        public Dictionary<string,MethodDefinition[]> DictNameField_Methods;
        public string version = ThisAssembly.Info.Version;    
    }
}

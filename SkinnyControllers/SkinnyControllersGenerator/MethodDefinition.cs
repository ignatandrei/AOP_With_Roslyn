using System;
using System.Collections.Generic;

namespace SkinnyControllersGenerator
{
    class MethodDefinition
    {
        public string ClassName { get; set; }
        public string Name { get; set; }

        public string ReturnType;
        public bool ReturnsVoid;
        //name, type
        public Dictionary<string, string> Parameters;

    }
}

using System;
using System.Collections.Generic;

namespace SkinnyControllersGenerator
{
    class MethodDefinition
    {
        public string ClassName { get; set; }
        public string Name { get; set; }

        public Type ReturnType;
        public bool ReturnsVoid;
        //name, type
        public Dictionary<string, string> Parameters;

    }
}

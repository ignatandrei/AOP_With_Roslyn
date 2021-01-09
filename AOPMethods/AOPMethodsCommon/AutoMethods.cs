using System;

namespace AOPMethodsCommon
{
    public enum TemplateMethod: long
    {

        None = 0,
        TryCatchConsole = 1,
        CustomTemplateFile = 10000,
    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoMethodsAttribute : Attribute
    {
        public TemplateMethod template { get; set; }
        public string MethodSuffix { get; set; }
        public string MethodPrefix { get; set; }

        public string[] ExcludeFields { get; set; }
        public string CustomTemplateFileName { get; set; }

    }

}

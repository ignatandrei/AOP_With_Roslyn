using System;

namespace AOPMethodsCommon
{
    public enum TemplateIndicator : long
    {

        None = 0,
        TryCatch = 1,
        CustomTemplateFile = 10000,
    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoMethodsAttribute : Attribute
    {
        public TemplateIndicator template { get; set; }
        public string[] MethodSuffix { get; set; }
        public string[] MethodPrefix { get; set; }
        public string CustomTemplateFileName { get; set; }

    }

}

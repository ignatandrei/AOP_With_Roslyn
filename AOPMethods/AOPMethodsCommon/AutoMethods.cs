﻿using System;

namespace AOPMethodsCommon
{
    public enum TemplateMethod: long
    {

        None = 0,
        TryCatchConsole = 1,
        CallerAtttributes=2,
        CallerAtttributesWithConsole=3,
        MethodWithPartial=4,
        CustomTemplateFile = 10000,
    }
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class AOPMarkerMethodAttribute : Attribute
    { 

    }
    public enum EnumMethod : long
    {

        None = 0,
        GenerateExtensionCode = 1,
        CustomTemplateFile = 10000,
    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class AutoMethodsAttribute : Attribute
    {
        public TemplateMethod template { get; set; }
        public string MethodSuffix { get; set; }
        public string MethodPrefix { get; set; }

        //public string[] ExcludeFields { get; set; }
        public string CustomTemplateFileName { get; set; }

    }
    [AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
    public class AutoEnumAttribute : Attribute
    {
        public EnumMethod  template { get; set; }
        public string CustomTemplateFileName { get; set; }

    }


}

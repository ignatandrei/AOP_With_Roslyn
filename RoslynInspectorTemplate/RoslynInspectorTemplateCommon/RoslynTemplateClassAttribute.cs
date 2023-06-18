using System;

namespace RoslynInspectorTemplateCommon;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class RoslynTemplateClassAttribute : Attribute
{
    public TemplateMethod template { get; set; }
    public InterceptDataOnClass InterceptDataOnClass { get; set; }

}


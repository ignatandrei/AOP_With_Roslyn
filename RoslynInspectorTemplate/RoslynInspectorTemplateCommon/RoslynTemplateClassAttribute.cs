using System;

namespace RoslynInspectorTemplateCommon;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class RoslynTemplateClassAttribute : Attribute
{
    public TemplateMethod template { get; set; }
    public InterceptDataOnClass InterceptDataOnClass { get; set; }

}


using System;

namespace RoslynInspectorTemplateCommon;

[Flags]
public enum InterceptDataOnClass
{
    None=0,
    Method=1,
    Property=2,
    Field=4
}

global using System;
global using System.Linq;
global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.CSharp;
global using Microsoft.CodeAnalysis.CSharp.Syntax;
global using System.Threading;


[Flags]
enum InterceptDataOnClass//todo: stay in sync!
{
    None = 0,
    Method = 1,
    Property = 2,
    Field = 4
}
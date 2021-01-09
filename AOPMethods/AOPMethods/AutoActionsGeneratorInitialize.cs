using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOPMethodsGenerator
{
    partial class AutoActionsGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {

            context.RegisterForSyntaxNotifications(() => new SyntaxReceiverClass());
            //in development
           // Debugger.Launch();
        }
    }
}

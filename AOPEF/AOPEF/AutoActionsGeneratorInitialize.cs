using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOPEFGenerator
{
    partial class AutoEFGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {

            context.RegisterForSyntaxNotifications(() => new SyntaxReceiverClass());
            //in development
            //Debugger.Launch();
        }
    }
}

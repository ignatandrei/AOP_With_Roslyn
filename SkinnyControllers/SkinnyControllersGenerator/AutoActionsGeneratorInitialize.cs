using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkinnyControllersGenerator
{
    partial class AutoActionsGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {

            context.RegisterForSyntaxNotifications(() => new SyntaxReceiverFields());
            //in development
            //Debugger.Launch();
        }
    }
}

using AOPEFCommon;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOPEFGenerator
{
    public class SyntaxReceiverClass : ISyntaxReceiver
    {
        string AutoEF = typeof(TemplateAttribute).Name;

        public List<ClassDeclarationSyntax> CandidatesClasses { get; } = new List<ClassDeclarationSyntax>();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                        && classDeclarationSyntax.AttributeLists.Count > 0)
            {
                bool found = false;
                foreach (var al in classDeclarationSyntax.AttributeLists)
                {
                    
                    var att = al.Attributes;
                    foreach(var at in att)
                    {
                        var x = at.Name as IdentifierNameSyntax;
                        if (x == null)
                            continue;
                        if (AutoEF.Contains(x.Identifier.Text))
                        {
                            CandidatesClasses.Add(classDeclarationSyntax);
                            found = true;
                        }
                        if (found)
                            break;
                    }
                    if (found)
                        break;

                }

            }

        }
    }
}

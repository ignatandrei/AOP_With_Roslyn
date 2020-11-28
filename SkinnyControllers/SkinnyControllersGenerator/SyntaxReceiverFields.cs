using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkinnyControllersGenerator
{
    public class SyntaxReceiverFields : ISyntaxReceiver
    {
        string autoActions = typeof(AutoActionsAttribute).Name;

        public SyntaxReceiverFields()
        {

        }
        public List<ClassDeclarationSyntax> CandidatesControllers { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                        && classDeclarationSyntax.AttributeLists.Count > 0)
            {
                foreach(var al in classDeclarationSyntax.AttributeLists)
                {
                    var att = al.Attributes;
                    foreach(var at in att)
                    {
                        var x = at.Name as IdentifierNameSyntax;
                        if (x == null)
                            continue;
                        if(autoActions.Contains(x.Identifier.Text))
                        {
                            CandidatesControllers.Add(classDeclarationSyntax);
                            return;
                        }
                    }
                }
                
            }

        }
    }
}

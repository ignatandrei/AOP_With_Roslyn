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
        public List<FieldDeclarationSyntax> CandidateFields { get; } = new List<FieldDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is FieldDeclarationSyntax fieldDeclarationSyntax
                        && fieldDeclarationSyntax.AttributeLists.Count > 0)
            {
                foreach(var al in fieldDeclarationSyntax.AttributeLists)
                {
                    var att = al.Attributes;
                    foreach(var at in att)
                    {
                        var x = at.Name as IdentifierNameSyntax;
                        if(autoActions.Contains(x.Identifier.Text))
                        {
                            CandidateFields.Add(fieldDeclarationSyntax);
                            return;
                        }
                    }
                }
                
            }

        }
    }
}

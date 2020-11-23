using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkinnyControllersGenerator
{
    public class SyntaxReceiverFields : ISyntaxReceiver
    {
        public List<FieldDeclarationSyntax> CandidateFields { get; } = new List<FieldDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is FieldDeclarationSyntax fieldDeclarationSyntax
                        && fieldDeclarationSyntax.AttributeLists.Count > 0)
            {
                //maybe find from here the AutoActionsAttribute
                CandidateFields.Add(fieldDeclarationSyntax);
            }

        }
    }
}

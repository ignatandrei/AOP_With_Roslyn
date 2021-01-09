using AOPMethodsCommon;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOPMethodsGenerator
{
    public class SyntaxReceiverClass : ISyntaxReceiver
    {
        string autoActions = typeof(AutoMethodsAttribute).Name;

        public SyntaxReceiverClass()
        {

        }
        public List<ClassDeclarationSyntax> CandidatesClasses { get; } = new List<ClassDeclarationSyntax>();
        public List<EnumDeclarationSyntax> CandidateEnums { get; } = new List<EnumDeclarationSyntax>();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if(syntaxNode is EnumDeclarationSyntax enums)
            {
                CandidateEnums.Add(enums);
            }
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
                        if (autoActions.Contains(x.Identifier.Text))
                        {
                            CandidatesClasses.Add(classDeclarationSyntax);
                            return;
                        }
                    }
                }
                
            }

        }
    }
}

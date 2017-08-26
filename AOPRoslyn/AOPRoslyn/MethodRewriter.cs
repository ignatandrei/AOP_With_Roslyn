using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace AOPRoslyn
{
    public class MethodRewriter: CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {


            if (node.Body == null || node.Body.Statements.Count == 0)
                return base.VisitMethodDeclaration(node);
            var parent = node.Parent as ClassDeclarationSyntax;
            if (parent == null)
                return base.VisitMethodDeclaration(node);

            var nameMethod = node.Identifier.Text;
            var nameClass = parent.Identifier.Text;
            Console.WriteLine(nameMethod);
            node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);
            var lineStart = node.GetLocation().GetLineSpan().StartLinePosition;
            string nameVariable = $"{nameClass}_{nameMethod}_{lineStart.Line}";
            var cmd = SyntaxFactory.ParseStatement($"Console.WriteLine(\"{nameVariable}\");//this is automatically added");

            var blockWithNewStatements = new SyntaxList<StatementSyntax>();
            
            for (int i = node.Body.Statements.Count - 1; i >= 0; i--)
            {
                var st = node.Body.Statements[i];
                blockWithNewStatements = blockWithNewStatements.Insert(0, st);
            }

            blockWithNewStatements = blockWithNewStatements.Insert(0, cmd);

            var newBlock = SyntaxFactory.Block(blockWithNewStatements);

            var newMethod = SyntaxFactory.MethodDeclaration
                (node.AttributeLists, node.Modifiers, node.ReturnType,
                node.ExplicitInterfaceSpecifier, node.Identifier, node.TypeParameterList,
                node.ParameterList, node.ConstraintClauses,
                newBlock,
                node.ExpressionBody, node.SemicolonToken);


            var newNode = node.ReplaceNode(node, newMethod);

            return base.VisitMethodDeclaration(newNode);
        }

    }
}

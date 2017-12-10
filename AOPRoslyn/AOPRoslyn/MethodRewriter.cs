using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using FormatWith;
namespace AOPRoslyn
{
    public class MethodRewriter: CSharpSyntaxRewriter
    {
       
        public MethodRewriter(string formatterFirstLine,string formatterLastLine=null)
        {
            FormatterFirstLine = formatterFirstLine;
            FormatterLastLine = formatterLastLine;
        }
        public string FormatterFirstLine { get; }
        public string FormatterLastLine { get; set; }
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

            StatementSyntax cmdFirstLine = null;
            if (FormatterFirstLine != null)
            {
                string firstLine = FormatterFirstLine.FormatWith(new { nameClass, nameMethod, lineStartNumber = lineStart.Line });
                cmdFirstLine = SyntaxFactory.ParseStatement(firstLine);
            }

            StatementSyntax cmdLastLine = null;
            if (FormatterLastLine != null)
            {
                string lastLine = FormatterLastLine.FormatWith(new { nameClass, nameMethod, lineStartNumber = lineStart.Line });
                cmdLastLine= SyntaxFactory.ParseStatement(lastLine);
            }
            var blockWithNewStatements = new SyntaxList<StatementSyntax>();
            if (cmdLastLine!= null)
                blockWithNewStatements = blockWithNewStatements.Insert(0, cmdLastLine);

            for (int i = node.Body.Statements.Count - 1; i >= 0; i--)
            {
                var st = node.Body.Statements[i];
                blockWithNewStatements = blockWithNewStatements.Insert(0, st);
            }
            if(cmdFirstLine != null)
                blockWithNewStatements = blockWithNewStatements.Insert(0, cmdFirstLine);


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

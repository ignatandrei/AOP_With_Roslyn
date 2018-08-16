using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using FormatWith;
using System.Linq;

namespace AOPRoslyn
{
    internal class MethodRewriter: CSharpSyntaxRewriter
    {
        private readonly AOPFormatter Formatter;

        public readonly RewriteOptions Options;
        public MethodRewriter(AOPFormatter formatter, RewriteOptions options)
        {
            Options = options;
            Formatter = formatter;
        }
        private static string TryToIdentifyParameter(ParameterSyntax p, AOPFormatter format)
        {
            string nameArgument = p.Identifier.Text;
            string typeArgument = null;
            var t = p.Type as PredefinedTypeSyntax;
            if (t != null)
            {
                typeArgument = t.Keyword.Text;
                
                
            }
            var i = p.Type as IdentifierNameSyntax;
            if(i != null)
            {
                
                typeArgument = i.Identifier.Text;
                
            }
            var a = p.Type as ArrayTypeSyntax;
            if(a != null)
            {
                typeArgument = a.ElementType.ToString();
                typeArgument = typeArgument + "[]";    
            }
            var q = p.Type as QualifiedNameSyntax;
            if(q!= null)
            {
                typeArgument = q.Right.Identifier.Text;
            }
            var g = p.Type as GenericNameSyntax;
            if(g != null)
            {
                typeArgument = g.ToFullString().Trim();
            }
            if(typeArgument == null)
            {
                typeArgument = "!"+p.Type.ToFullString();
            }
            var str = format.FormattedText(typeArgument);
            
            if (str == null)
            {
                str = format.DefaultFormattedText();
            }
            if(str == null)
                return str;

            str= str.FormatWith(new { item = nameArgument, itemtype = typeArgument });
            return str;

            //string full = nameArgument + " " + typeArgument;
            
        }
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {


            if (node.Body == null || node.Body.Statements.Count == 0)
                return base.VisitMethodDeclaration(node);
            var parent = node.Parent as ClassDeclarationSyntax;
            if (parent == null)
                return base.VisitMethodDeclaration(node);

            var nameMethod = node.Identifier.Text;
            var nameClass = parent.Identifier.Text;

            //dotnet-aop-uncomment var cc = System.Console.ForegroundColor;
            //dotnet-aop-uncomment System.Console.ForegroundColor = ConsoleColor.Yellow;
            //dotnet-aop-uncomment System.Console.WriteLine($"processing method {nameMethod} from class {nameClass}");
            //dotnet-aop-uncomment System.Console.ForegroundColor =cc;

            string arguments = "";
            //if (Options.WriteArguments)
            {
                var parameters = node.ParameterList.Parameters;
                if (parameters.Count == 0)
                {
                    arguments = this.Options.NoArguments;
                }
                else
                {
                    var l = parameters.Count;
                    var argsArray = new string[l];
                    for (int i = 0; i < l; i++)
                    {
                        argsArray[i]=TryToIdentifyParameter(parameters[i], Formatter);
                    }
                    var data = argsArray.Where(it => it != null).ToArray();
                    if (data.Length > 0)
                        arguments = string.Join(Options.ArgumentSeparator, data);
                    else
                        arguments = this.Options.NoArguments;
                }
            }
            node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);
            var lineStart = node.GetLocation().GetLineSpan().StartLinePosition;

            StatementSyntax cmdFirstLine = null;
            if (Formatter.FormatterFirstLine != null)
            {
                string firstLine = Formatter.FormatterFirstLine.FormatWith(new { nameClass, nameMethod, lineStartNumber = lineStart.Line , arguments});
                cmdFirstLine = SyntaxFactory.ParseStatement(firstLine);

            }

            StatementSyntax cmdLastLine = null;
            if (Formatter.FormatterLastLine != null)
            {
                string lastLine = Formatter.FormatterLastLine.FormatWith(new { nameClass, nameMethod, lineStartNumber = lineStart.Line });
                cmdLastLine= SyntaxFactory.ParseStatement(lastLine);
            }
            var blockWithNewStatements = new SyntaxList<StatementSyntax>();
            if (cmdLastLine!= null)
                blockWithNewStatements = blockWithNewStatements.Insert(0, cmdLastLine);

            for (int i = node.Body.Statements.Count - 1; i >= 0; i--)
            {
                var st = node.Body.Statements[i];
                if(Options.PreserveLinesNumber && i == 0  )
                {
                    var line = st.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                    var lit = SyntaxFactory.Literal(line.ToString(), line);
                    var ld = SyntaxFactory.LineDirectiveTrivia(lit, true);
                    var stLine= SyntaxFactory.Trivia(ld);                    
                    st = st.WithLeadingTrivia(SyntaxFactory.CarriageReturnLineFeed, stLine, SyntaxFactory.CarriageReturnLineFeed);

                }
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

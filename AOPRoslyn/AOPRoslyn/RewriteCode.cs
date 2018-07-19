using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AOPRoslyn
{
    public class RewriteCode
    {
        public RewriteCode(): this("Console.WriteLine(\"{nameClass}_{nameMethod}_{lineStartNumber}\");")
        {

        }
        public bool PreserveLinesNumber { get; set; } = true;
        public RewriteCode(string formatterFirstLine,string formatterLastLine=null)
        {
            FormatterFirstLine = formatterFirstLine;
            FormatterLastLine = formatterLastLine;
        }
        public string Code { get; set; }
        public string FormatterFirstLine { get; }
        public string FormatterLastLine { get; }

        public string RewriteCodeMethod()
        {
            var tree = CSharpSyntaxTree.ParseText(Code);
            
            var node = tree.GetRoot();
            node=ModifyRegionToTrivia(node);
            var LG = new MethodRewriter(FormatterFirstLine,FormatterLastLine);
            LG.PreserveLinesNumber = PreserveLinesNumber;
            var sn = LG.Visit(node);
            return sn.NormalizeWhitespace().ToFullString();

        }

        private SyntaxNode ModifyRegionToTrivia(SyntaxNode syntaxNode)
        {
            while (true)
            {
                var trivia = syntaxNode.DescendantTrivia(null, false)
                    .Select(it=>(SyntaxTrivia)it)
                    .Select(it=>new { it= it,Kind=it.Kind() })
                    .ToArray()
                    .FirstOrDefault(t => t.Kind == SyntaxKind.RegionDirectiveTrivia || t.Kind == SyntaxKind.EndRegionDirectiveTrivia);
                if (trivia == null || trivia.Kind == SyntaxKind.None)
                {
                    break;
                }
                syntaxNode = syntaxNode.ReplaceTrivia(trivia.it, SyntaxFactory.Comment("//was a region " + Environment.NewLine));
            }
            return syntaxNode;
        }
    }
}

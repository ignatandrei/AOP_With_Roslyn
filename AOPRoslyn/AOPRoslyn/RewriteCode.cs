using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AOPRoslyn
{
    public class RewriteCode
    {
        public static readonly string firstLineMethod = "Console.WriteLine(\"start {nameClass}_{nameMethod}_{lineStartNumber}\");";
        public static readonly string lastLineMethod = "Console.WriteLine(\"end {nameClass}_{nameMethod}_{lineStartNumber}\");";
        public RewriteCode() : this(firstLineMethod, lastLineMethod)
        {

        }
        public bool PreserveLinesNumber { get; set; } = true;
        public RewriteCode(string formatterFirstLine,string formatterLastLine)
        {
            FormatterFirstLine = formatterFirstLine;
            FormatterLastLine = formatterLastLine;
        }
        public string Code { get; set; }
        public string FormatterFirstLine { get; }
        public string FormatterLastLine { get; }

        public virtual string RewriteCodeMethod()
        {
            var tree = CSharpSyntaxTree.ParseText(Code);
            
            var node = tree.GetRoot();
            node=ModifyRegionToTrivia(node);
            var LG = new MethodRewriter(FormatterFirstLine,FormatterLastLine);
            LG.PreserveLinesNumber = PreserveLinesNumber;
            var sn = LG.Visit(node);
            var data= sn.ToFullString();
            //BUG - cannot have this space between #line and number
            data = data.Replace(Environment.NewLine + "#line", Environment.NewLine + "#line ");
            return data;
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

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AOPRoslyn
{
    /// <summary>
    /// default worker implementation
    /// </summary>
    public class RewriteCode
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public RewriteCode() : this(AOPFormatter.DefaultFormatter)
        {

        }
        /// <summary>
        /// the formatter
        /// </summary>
        public AOPFormatter Formatter { get; internal set; }
        /// <summary>
        /// options to format
        /// </summary>
        public RewriteOptions Options { get; internal set; }
        /// <summary>
        /// constructor with formatter and default options
        /// </summary>
        /// <param name="formatter"></param>
        public RewriteCode(AOPFormatter formatter)
        {
            Formatter = formatter;
            Options = new RewriteOptions();
        }
        /// <summary>
        /// the Code to be AOP'ed
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// the main action 
        /// </summary>
        /// <returns>code AOP'ed</returns>
        public virtual string RewriteCodeMethod()
        {
            var tree = CSharpSyntaxTree.ParseText(Code);
            
            var node = tree.GetRoot();
            node=ModifyRegionToTrivia(node);
            node = ModifyDotNetAOPComments(node);
            var LG = new MethodRewriter(Formatter, Options);
                    
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
        private SyntaxNode ModifyDotNetAOPComments(SyntaxNode sn)
        {
            var triviaReplace = new Dictionary<SyntaxTrivia, SyntaxTrivia>();
            foreach (SyntaxTrivia item in sn.DescendantTrivia(null, false))
            {
                if (item.RawKind == (int)SyntaxKind.SingleLineCommentTrivia)
                {
                    string text = item.ToFullString();
                    if (text.ToLower().Contains("dotnet-aop-uncomment"))
                    {
                        string valueText= text.Replace("//dotnet-aop-uncomment", "");

                        triviaReplace.Add(item, SyntaxFactory.SyntaxTrivia(SyntaxKind.SingleLineCommentTrivia, valueText));
                    }
                }
            }
            if (triviaReplace.Count == 0)
                return sn;

            return sn.ReplaceTrivia((from it in triviaReplace
                                     select it.Key).ToArray(), (SyntaxTrivia x, SyntaxTrivia y) => triviaReplace[x]);

        }
    }
}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AOPRoslyn
{
    public class RewriteCode
    {
        public RewriteCode()
        {

        }
        public string Code { get; set; }

        public string RewriteCodeMethod()
        {
            var tree = CSharpSyntaxTree.ParseText(Code);

            var node = tree.GetRoot();

            var LG = new MethodRewriter();
            var sn = LG.Visit(node);
            return sn.NormalizeWhitespace().ToFullString();

        }
    }
}

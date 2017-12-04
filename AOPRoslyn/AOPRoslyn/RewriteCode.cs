using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AOPRoslyn
{
    public class RewriteCode
    {
        public RewriteCode(): this("Console.WriteLine(\"{nameClass}_{nameMethod}_{lineStartNumber}\");")
        {

        }
        public RewriteCode(string formatter)
        {
            Formatter = formatter;
        }
        public string Code { get; set; }
        public string Formatter { get; }

        public string RewriteCodeMethod()
        {
            var tree = CSharpSyntaxTree.ParseText(Code);

            var node = tree.GetRoot();

            var LG = new MethodRewriter(Formatter);
            var sn = LG.Visit(node);
            return sn.NormalizeWhitespace().ToFullString();

        }
    }
}

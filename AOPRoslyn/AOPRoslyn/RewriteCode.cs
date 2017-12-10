using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AOPRoslyn
{
    public class RewriteCode
    {
        public RewriteCode(): this("Console.WriteLine(\"{nameClass}_{nameMethod}_{lineStartNumber}\");")
        {

        }
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

            var LG = new MethodRewriter(FormatterFirstLine,FormatterLastLine);
            var sn = LG.Visit(node);
            return sn.NormalizeWhitespace().ToFullString();

        }
    }
}

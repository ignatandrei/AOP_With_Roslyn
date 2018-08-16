using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AOPRoslyn
{
    public class RewriteCodeFile : RewriteAction
    {
        public RewriteCodeFile() : this(AOPFormatter.DefaultFormatter, null)
        {
        }
        public RewriteCodeFile(string fileName): this(AOPFormatter.DefaultFormatter,fileName)
        {
        }
        public RewriteCodeFile(AOPFormatter formatter, string fileName)
        {
            rc = new RewriteCode(formatter);           
            FileName = fileName;
        }
        private RewriteCode rc { get; set; }
        public string FileName { get;  set; }
        public override void Rewrite()
        {

            var Code = File.ReadAllText(FileName);
            //dotnet-aop-uncomment var cc = System.Console.ForegroundColor;
            //dotnet-aop-uncomment System.Console.ForegroundColor = ConsoleColor.Red;
            //dotnet-aop-uncomment System.Console.WriteLine($"processing " + FileName);
            //dotnet-aop-uncomment System.Console.ForegroundColor =cc;
            if (string.IsNullOrWhiteSpace(Code))
                return;
            rc.Code = Code;
            rc.Formatter = Formatter;
            rc.Options = Options;
            FileInfo fi = new FileInfo(FileName);
            if (fi.IsReadOnly)
                fi.IsReadOnly = false;

            File.WriteAllText(FileName, rc.RewriteCodeMethod());

        }
    }
}
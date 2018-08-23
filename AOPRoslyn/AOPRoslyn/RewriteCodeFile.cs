using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AOPRoslyn
{
    /// <summary>
    /// rewriting a file
    /// </summary>
    public class RewriteCodeFile : RewriteAction
    {
        /// <summary>
        /// default constructor with formatter
        /// without specifying the name file 
        /// </summary>
        public RewriteCodeFile() : this(AOPFormatter.DefaultFormatter, null)
        {
        }
        /// <summary>
        /// default formatter, name file specified
        /// </summary>
        /// <param name="fileName"></param>
        public RewriteCodeFile(string fileName): this(AOPFormatter.DefaultFormatter,fileName)
        {
        }
        /// <summary>
        /// constructor for file and formatter
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="fileName"></param>
        public RewriteCodeFile(AOPFormatter formatter, string fileName)
        {
            rc = new RewriteCode(formatter);           
            FileName = fileName;
        }
        private RewriteCode rc { get; set; }
        /// <summary>
        /// the file name to be AOP'ed
        /// </summary>
        public string FileName { get;  set; }
        /// <summary>
        /// the action to AOP
        /// </summary>
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
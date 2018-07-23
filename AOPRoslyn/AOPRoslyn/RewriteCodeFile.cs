using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AOPRoslyn
{
    public class RewriteCodeFile : IRewriteAction
    {
        RewriteCode rc;
        public RewriteCodeFile(string fileName): this(RewriteCode.firstLineMethod,RewriteCode.lastLineMethod,fileName)
        {
        }
        public RewriteCodeFile(string formatterFirstLine, string formatterLastLine, string fileName)
        {
            rc = new RewriteCode(formatterFirstLine, formatterLastLine);
            FileName = fileName;
        }

        public string FileName { get; internal set; }
        public void Rewrite()
        {
            var Code = File.ReadAllText(FileName);
            if (string.IsNullOrWhiteSpace(Code))
                return;
            rc.Code = Code;
            FileInfo fi = new FileInfo(FileName);
            if (fi.IsReadOnly)
                fi.IsReadOnly = false;

            File.WriteAllText(FileName, rc.RewriteCodeMethod());

        }
    }
}
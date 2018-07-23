using System;
using System.IO;

namespace AOPRoslyn
{
    public class RewriteCodeFolder: IRewriteAction
    {
        public event EventHandler<string> EndProcessingFile;
        public event EventHandler<string> StartProcessingFile;
        public RewriteCodeFolder( string folderName, string searchPattern) : 
            this(RewriteCode.firstLineMethod, RewriteCode.lastLineMethod, folderName, searchPattern)
        {

        }
        public RewriteCodeFolder(string formatterFirstLine, string formatterLastLine, string folderName, string searchPattern) 
        {
            FormatterFirstLine = formatterFirstLine;
            FormatterLastLine = formatterLastLine;
            FolderName = folderName;
            SearchPattern = searchPattern;
            
        }

        public string FormatterFirstLine { get; }
        public string FormatterLastLine { get; }
        public string FolderName { get; }
        public string SearchPattern { get; }

        public void Rewrite()
        {
            var rc = new RewriteCodeFile(FormatterFirstLine, FormatterLastLine, null);
            foreach (var item in Directory.EnumerateFiles(FolderName, SearchPattern, SearchOption.AllDirectories))
            {
                if (StartProcessingFile != null) //TODO: make a bool
                    StartProcessingFile(this, item);
                rc.FileName = item;
                rc.Rewrite();
                if (EndProcessingFile != null)//TODO: make a bool
                    EndProcessingFile(this, item);

            }
        }
    }
}

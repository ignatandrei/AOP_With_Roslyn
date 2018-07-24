using System;
using System.IO;
using System.Linq;

namespace AOPRoslyn
{
    public class RewriteCodeFolder: IRewriteAction
    {
        public event EventHandler<string> EndProcessingFile;
        public event EventHandler<string> StartProcessingFile;
        public RewriteCodeFolder( string folderName, string searchPattern) : 
            this(AOPFormatter.DefaultFormatter, folderName, searchPattern)
        {

        }
        public RewriteCodeFolder(AOPFormatter formatter, string folderName, string searchPattern) 
        {
            
            FolderName = folderName;
            SearchPattern = searchPattern;
            ExcludeFileNames = new string[0];
            Formatter = formatter;
        }
        public string[] ExcludeFileNames;
      
        public string FolderName { get; }
        public string SearchPattern { get; }
        public AOPFormatter Formatter { get; }
        public void Rewrite()
        {
            var rc = new RewriteCodeFile(Formatter, null);
            foreach (var item in Directory.EnumerateFiles(FolderName, SearchPattern, SearchOption.AllDirectories))
            {
                if (ExcludeFileNames.Contains(Path.GetFileName(item)))
                    continue;

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

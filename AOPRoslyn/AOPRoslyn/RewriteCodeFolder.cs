using System;
using System.IO;
using System.Linq;

namespace AOPRoslyn
{
    public class RewriteCodeFolder: RewriteAction
    {
        public event EventHandler<string> EndProcessingFile;
        public event EventHandler<string> StartProcessingFile;
        public RewriteCodeFolder() : this(null, null)
        {
           
        }
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
            Options = new RewriteOptions();

        }
        public string[] ExcludeFileNames { get; set; }

        public string FolderName { get; set; }
        public string SearchPattern { get; set; }
        
        public override void Rewrite()
        {
            var rc = new RewriteCodeFile(Formatter, null);
            //dotnet-aop-uncomment System.Console.WriteLine($"processing " + FolderName);
            rc.Options = Options;
            rc.Formatter = Formatter;
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

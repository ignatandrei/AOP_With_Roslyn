using System;
using System.IO;
using System.Linq;

namespace AOPRoslyn
{
    /// <summary>
    /// rewrite all files in the folder, recursively
    /// </summary>
    public class RewriteCodeFolder: RewriteAction
    {
        /// <summary>
        /// after processing the file
        /// </summary>
        public event EventHandler<string> EndProcessingFile;
        /// <summary>
        /// before processing the file
        /// </summary>
        public event EventHandler<string> StartProcessingFile;
        /// <summary>
        /// default constructor
        /// </summary>
        public RewriteCodeFolder() : this(null, null)
        {
           
        }
        /// <summary>
        /// constructor with default formatter
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="searchPattern"></param>
        public RewriteCodeFolder( string folderName, string searchPattern) : 
            this(AOPFormatter.DefaultFormatter, folderName, searchPattern)
        {

        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="formatter"></param>
        /// <param name="folderName"></param>
        /// <param name="searchPattern"></param>
        public RewriteCodeFolder(AOPFormatter formatter, string folderName, string searchPattern) 
        {
            
            FolderName = folderName;
            SearchPattern = searchPattern;
            ExcludeFileNames = new string[0];
            Formatter = formatter;
            Options = new RewriteOptions();

        }
        /// <summary>
        /// what files to exclude
        /// prevent logging adding to logging code
        /// </summary>
        public string[] ExcludeFileNames { get; set; }
        /// <summary>
        /// what folder to search ( recursively) 
        /// for files
        /// </summary>
        public string FolderName { get; set; }
        /// <summary>
        /// search for file ( default *.cs)
        /// </summary>
        public string SearchPattern { get; set; }
        /// <summary>
        /// rewrite action
        /// </summary>
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

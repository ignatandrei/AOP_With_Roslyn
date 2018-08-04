namespace AOPRoslyn
{
    public class RewriteOptions
    {
        public bool PreserveLinesNumber { get; set; } = true;
        public string NoArguments { get; set; } = "\"No arguments in method\"";
        public string ArgumentSeparator { get; set; } = "+";
    }
}

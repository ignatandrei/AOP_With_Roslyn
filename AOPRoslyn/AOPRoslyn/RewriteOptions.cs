namespace AOPRoslyn
{
    /// <summary>
    /// how to rewrite code
    /// </summary>
    public class RewriteOptions
    {
        /// <summary>
        /// if true then inserts a #line directive
        /// </summary>
        public bool PreserveLinesNumber { get; set; } = true;
        /// <summary>
        /// if the method has no arguments, put this text
        /// </summary>
        public string NoArguments { get; set; } = "\"No arguments in method\"";
        /// <summary>
        /// seaprator between arguments
        /// </summary>
        public string ArgumentSeparator { get; set; } = "+";
    }
}

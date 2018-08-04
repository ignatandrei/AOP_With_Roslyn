using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOPRoslyn
{
    [DebuggerDisplay("AOPFormatter= {DebugDisplay()}")]
    public class AOPFormatter
    {
        public AOPFormatter()
        { 
            FormatArguments = new Dictionary<string, string>();
            //TODO: add datetime, guid
            //TODO: see stankins


        }
        public string DebugDisplay()
        {
            return $"Number arguments {FormatArguments?.ToString()} ";
        }
        public static readonly string firstLineMethod = "System.Console.WriteLine(\"start {nameClass}_{nameMethod}_{lineStartNumber}\");";
        public static readonly string lastLineMethod = "System.Console.WriteLine(\"end {nameClass}_{nameMethod}_{lineStartNumber}\");";
        public bool AddDefaultArguments { get; set; } = true;
        //TODO: Make singleton
        public static AOPFormatter DefaultFormatter {
            get
            {
                return new AOPFormatter()
                {
                    FormatterFirstLine = firstLineMethod,
                    FormatterLastLine = lastLineMethod
                };
            }
        }
        public string FormatterFirstLine { get; set; }
        public string FormatterLastLine { get; set; }

        private bool AddedOnce = false;
        private void AddDefaultArgumentsOnce()
        {
            if (AddedOnce)
                return;

            AddIfNotExists("*", "\"no identifiable argument type {itemtype} \"");
            AddIfNotExists("string", "\"{item}=\"+({item}??\"\").ToString()");
            AddIfNotExists("ParameterSyntax", "\"{item}=\"+{item}?.ToString()");
            AddIfNotExists("MethodDeclarationSyntax", "\"{item}=\"+{item}?.ToFullString()");
            AddIfNotExists("SyntaxNode", "\"{item}=\"+{item}?.ToFullString()");
            AddIfNotExists("Compilation", "\"{item}=\"+{item}?.AssemblyName");


            AddedOnce = true;
        }
        private void AddIfNotExists(string key, string value)
        {
            if (FormatArguments.ContainsKey(key))
                return;
            FormatArguments.Add(key, value);
        }
        public Dictionary<string, string> FormatArguments;
        internal string DefaultFormattedText()
        {
            if (FormatArguments.ContainsKey("*"))
                return FormatArguments["*"];

            return null;
        }
        internal string FormattedText(string type)
        {
            if(AddDefaultArguments & !AddedOnce)
                AddDefaultArgumentsOnce();

            if (FormatArguments.ContainsKey(type))
                return FormatArguments[type]; 
            
            return null;

        }
    }
}

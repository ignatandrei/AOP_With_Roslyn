using System;
using System.Collections.Generic;
using System.Text;

namespace AOPRoslyn
{
    public class AOPFormatter
    {
        public AOPFormatter()
        { 
            FormatArguments = new Dictionary<string, string>();
            FormatArguments.Add("string", "({item}??\"\").ToString()");
            FormatArguments.Add("*", "({item}??default({itemtype})).ToString()");

        }
        public static readonly string firstLineMethod = "Console.WriteLine(\"start {nameClass}_{nameMethod}_{lineStartNumber}\");";
        public static readonly string lastLineMethod = "Console.WriteLine(\"end {nameClass}_{nameMethod}_{lineStartNumber}\");";

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

        //type with text
        public Dictionary<string, string> FormatArguments;
        public string FormattedText(string type)
        {
            if (FormatArguments.ContainsKey(type))
                return FormatArguments[type];

            if (FormatArguments.ContainsKey("*"))
                return FormatArguments["*"];

            return null;

        }
    }
}

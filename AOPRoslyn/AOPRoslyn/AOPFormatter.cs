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
            //TODO: add datetime, guid
            FormatArguments.Add(ArgumentType.Class.ToString(), "({item}??default({itemtype})).ToString()");
            FormatArguments.Add(ArgumentType.ValueType.ToString(), "{item}.ToString()");


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
        internal string FormattedText(string type, ArgumentType arg)
        {
            if (FormatArguments.ContainsKey(type))
                return FormatArguments[type];

            if (FormatArguments.ContainsKey(arg.ToString()))
                return FormatArguments[arg.ToString()];

            return null;

        }
    }
}

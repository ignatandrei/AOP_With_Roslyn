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
                    FormatterLastLine = lastLineMethod,
                    AddDefaultArguments = true
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
            AddIfNotExists("bool", "\"{item}=\"+{item}.ToString()");
            AddIfNotExists("int", "\"{item}=\"+{item}.ToString()");
            AddIfNotExists("long", "\"{item}=\"+{item}.ToString()");
            AddIfNotExists("ParameterSyntax", "\"{item}=\"+{item}?.ToString()");
            AddIfNotExists("MethodDeclarationSyntax", "\"{item}=\"+{item}?.Identifier.Text");
            AddIfNotExists("SyntaxNode", "\"{item}=\"+((SyntaxKind){item}?.RawKind)");
            AddIfNotExists("Compilation", "\"{item}=\"+{item}?.AssemblyName");
            AddIfNotExists("object", "\"{item}=\"+{item}?.ToString()");
            AddIfNotExists("JsonWriter", "\"{item}=\"+{item}?.Path");
            AddIfNotExists("JsonReader", "\"{item}=\"+{item}?.Path");
            AddIfNotExists("Type", "\"{item}=\"+{item}?.FullName");
            AddIfNotExists("DbCommand", "\"{item}=\"+{item}?.CommandText");
            AddIfNotExists("Assembly","\"{item}=\"+{item}?.FullName");
            AddIfNotExists("TypeInfo", "\"{item}=\"+{item}?.FullName");
            AddIfNotExists("Exception", "\"{item}=\"+{item}?.ToString()");
            AddIfNotExists("EventArgs", "\"{item}=\"+{item}.ToString()");
            AddIfNotExists("ParameterInfo", "\"{item}=\"+{item}.Name");
            AddIfNotExists("CultureInfo", "\"{item}=\"+{item}.Name");
            AddIfNotExists("DirectoryInfo", "\"{item}=\"+{item}.Name");
            AddIfNotExists("AssemblyName", "\"{item}=\"+{item}.Name");
            AddIfNotExists("[]", "\"{item}.Count=\"+{item}?.Length");


            //public static bool CompareDictionary(Dictionary<string, object> x, Dictionary<string, object> y)
            //JsonSerializer
            //IServiceCollection
            //Dictionary<string, object> obj
            //string[]
            //ActionContext
            //object[]
            //ITypeDescriptorContext 
            //Regex 
            //Match 
            //AssemblyLoadContext 
            //List<ValuesToTranslate> 
            //char
            //byte[]
            //KeyValuePair<string, string> 
            //IHostingEnvironment 
            //ErrorEventArgs
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
            if (AddDefaultArguments & !AddedOnce)
                AddDefaultArgumentsOnce();

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

            if (type.EndsWith("[]") && FormatArguments.ContainsKey("[]"))
                return FormatArguments["[]"];

            return null;

        }
    }
}

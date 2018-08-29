using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AOPRoslyn
{
    /// <summary>
    /// Formatter arguments and more for AOP
    /// </summary>
    [DebuggerDisplay("AOPFormatter= {DebugDisplay()}")]
    public class AOPFormatter
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public AOPFormatter()
        { 
            FormatArguments = new Dictionary<string, string>();
            //TODO: add datetime, guid
            //TODO: see stankins
            MethodsToLog = ModifierMethod.All;


        }
        /// <summary>
        /// just debugger string
        /// </summary>
        /// <returns>debugger string</returns>
        public string DebugDisplay()
        {
            return $"Number arguments {FormatArguments?.ToString()} ";
        }
        /// <summary>
        /// default first line to be inserted
        /// </summary>
        public static readonly string firstLineMethod = "System.Console.WriteLine(\"start {nameClass}_{nameMethod}_{lineStartNumber}\");";
        /// <summary>
        /// default last line to be inserted
        /// </summary>
        public static readonly string lastLineMethod = "System.Console.WriteLine(\"end {nameClass}_{nameMethod}_{lineStartNumber}\");";
        /// <summary>
        /// adding default arguments  
        /// improving with each version
        /// </summary>
        public bool AddDefaultArguments { get; set; } = true;
        
        /// <summary>
        /// TODO: Make singleton
        /// default formatter
        /// </summary>
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
        /// <summary>
        /// first line to be inserted( supports new line)
        /// </summary>
        public string FormatterFirstLine { get; set; }
        /// <summary>
        /// last line to be inserted( supports new line)
        /// </summary>
        public string FormatterLastLine { get; set; }
        /// <summary>
        /// what are the methods to log
        /// support bitwise flags
        /// </summary>
        public ModifierMethod MethodsToLog { get; set; }
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
            AddIfNotExists("ModifierMethod", "\"{item}=\"+{item}.ToString()");
            AddIfNotExists("SyntaxKind", "\"{item}=\"+{item}.ToString()");
            

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
        /// <summary>
        /// how to format various arguments
        /// </summary>
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

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using AOPRoslyn;
using InterpreterDll;
using McMaster.Extensions.CommandLineUtils;

namespace aop
{
    //TODO: add debug own version
    [Command(Description = "Simple dot net aop.")]
    class Program
    {
        public static int Main(string[] args)
        {
            if(args.Length == 0)
            {
                var pathDll = Assembly.GetEntryAssembly().Location;
                var path = Path.GetDirectoryName(pathDll);
                var pathFile = Path.Combine(path, "processme.txt");
                Console.WriteLine($" taking default process me on {pathFile}");
                args = new string[] { pathFile };
            }
            return CommandLineApplication.Execute<Program>(args);
        }

        [Argument(0, Description = "The settings file to aop.\nYou can find a processme.txt near the executable.")]
        [Required]
        public string Name { get; }

        
        private int OnExecute()
        {
            Console.WriteLine($"processing files accordingly to settings from {Name}");
            var i = new Interpret();
            var text = i.InterpretText(File.ReadAllText(Name));
            var rewrite = RewriteAction.UnSerializeMe(text);
            rewrite.Rewrite();            
            return 0;
            //for (var i = 0; i < Count; i++)
            //{
            //    Console.WriteLine($"Hello {Name}!");
            //}
            //return 0;
        }
    }
}

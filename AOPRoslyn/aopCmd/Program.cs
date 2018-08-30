using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using AOPRoslyn;
using AOPStatistics;
using ConsoleTables;
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
                //Taking processme.txt from current directory
                var pathFile = Path.Combine(Environment.CurrentDirectory, "processme.txt");
                if (!File.Exists(pathFile))
                {

                    var pathDll = Assembly.GetEntryAssembly().Location;
                    var path = Path.GetDirectoryName(pathDll);
                    pathFile = Path.Combine(path, "processme.txt");
                }
                Console.WriteLine($"no arguments name file, taking default processme.txt file from on {pathFile}");                
                args = new string[] { pathFile };
            }
            var ret= CommandLineApplication.Execute<Program>(args);
            var table = new ConsoleTable("Class", "Method(Line)", "NumberHit", "TotalTime", "AverageTime");
            if (GatherStatistics.timingMethod.Count == 0)
            {
                Console.WriteLine("no statistics loaded");
                return ret;
            }
            var data = GatherStatistics.DataGathered()
                        .OrderByDescending(it => it.TotalDuration / it.NumberHits)
                        .ToArray();

            foreach (var item in data)
            {
                table.AddRow(item.m.className, $"{item.m.methodName}({item.m.line})", item.NumberHits, item.TotalDuration, (item.TotalDuration / item.NumberHits));
            }
            table.Write(Format.Alternative);
            return ret;
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

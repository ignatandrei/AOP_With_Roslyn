using System;
using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace aop
{
    [Command(Description = "Simple dot net aop.")]
    class Program
    {
        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        [Argument(0, Description = "The settings file to aop.\nYou can find a sample in the example .")]
        [Required]
        public string Name { get; }

        //[Option(Description = "An optional parameter, with a default value.\nThe number of times to say hello.")]
        //[Range(1, 1000)]
        //public int Count { get; } = 1;

        private int OnExecute()
        {
            return 0;
            //for (var i = 0; i < Count; i++)
            //{
            //    Console.WriteLine($"Hello {Name}!");
            //}
            //return 0;
        }
    }
}

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;

namespace AOPBenchMark
{
    class Program
    {
        static void Main(string[] args)
        {
            var email = new EmailSmtpClientMSOneProperty();
            email.Setup();
            Console.WriteLine(email.GetHostReflection());
            Console.WriteLine(email.GetHostViaDictionary());
            Console.WriteLine(email.GetHostViaSwitch());
            BenchmarkRunner.Run<EmailSmtpClientMSOneProperty>(
                ManualConfig
                    .Create(DefaultConfig.Instance)
                    .WithOption(ConfigOptions.DisableOptimizationsValidator, true)
                    );

            BenchmarkRunner.Run<EmailSmtpClientMSMultipleProperties>(
                ManualConfig
                    .Create(DefaultConfig.Instance)
                    .WithOption(ConfigOptions.DisableOptimizationsValidator, true)
                    );


        }
    }
}

using System;

namespace AOPBenchMark
{
    class Program
    {
        static void Main(string[] args)
        {
            var email = new EmailSmtpClientMS();
            email.Host = "http://msprogrammer.serviciipeweb.ro/";
            Console.WriteLine(email.GetHostReflection());
            Console.WriteLine(email.GetHostViaDictionary());
            Console.WriteLine(email.GetHostViaSwitch());

        }
    }
}

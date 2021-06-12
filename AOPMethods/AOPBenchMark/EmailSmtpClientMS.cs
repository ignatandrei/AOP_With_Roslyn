using AOPMethodsCommon;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Linq;

namespace AOPBenchMark
{
    [SimpleJob(RuntimeMoniker.Net50)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [RPlotExporter]
    [AutoMethods(template = TemplateMethod.CustomTemplateFile, CustomTemplateFileName = "ClassToDictionary.txt")]

    public partial class EmailSmtpClientMS 
    {
        [Benchmark]
        public string GetHostReflection()
        {
            return this.GetType().GetProperty("Host").GetValue(this).ToString();
        }
        [Benchmark]
        public string GetHostViaDictionary()
        {
            return this.ReadMyProperties().First(it => it.Key == "Host").Value.ToString();
        }
        [Benchmark]
        public string GetHostViaSwitch()
        {
            return this.GetValueProperty("Host").ToString();
        }
        [GlobalSetup]
        public void Setup()
        {
            Host = "http://msprogrammer.serviciipeweb.ro/";

        }
        public EmailSmtpClientMS()
        {

            Port = 25;

        }
        public string Name { get; set; }


        public string Type
        {
            get
            {
                return this.GetType().Name;
            }
        }
        public string Host { get; set; }
        public int Port { get; set; }

        public string Description
        {
            get
            {
                return $"{Type} {Host}:{Port}";
            }
        }
    }
}

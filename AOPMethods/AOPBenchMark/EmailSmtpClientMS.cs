using AOPMethodsCommon;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using System.Linq;

namespace AOPBenchMark
{
    [SimpleJob(RuntimeMoniker.Net50)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [RPlotExporter]
    [CsvMeasurementsExporter]
    [MemoryDiagnoser]
    [HtmlExporter]
    [MarkdownExporterAttribute.GitHub]
    [AutoMethods(template = TemplateMethod.CustomTemplateFile, CustomTemplateFileName = "ClassToDictionary.txt")]

    public partial class EmailSmtpClientMS 
    {
        
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

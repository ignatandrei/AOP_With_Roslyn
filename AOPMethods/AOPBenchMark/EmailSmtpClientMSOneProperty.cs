using BenchmarkDotNet.Attributes;

namespace AOPBenchMark
{


    public partial class EmailSmtpClientMSOneProperty: EmailSmtpClientMS
    {
        [Benchmark]
        public string GetHostReflection()
        {
            return this.GetType().GetProperty("Host").GetValue(this).ToString();
        }
        [Benchmark]
        public string GetHostViaDictionary()
        {
            return this.ReadMyProperties()["Host"].ToString();
        }
        [Benchmark]
        public string GetHostViaSwitch()
        {
            return this.GetValueProperty("Host").ToString();
        }
    }
}

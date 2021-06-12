using BenchmarkDotNet.Attributes;
using System.Linq;

namespace AOPBenchMark
{
    public partial class EmailSmtpClientMSMultipleProperties : EmailSmtpClientMS
    {
        
        [Benchmark]
        public string GetHostReflection()
        {
            var props = this.GetType()
                .GetProperties()
                .ToDictionary(it => it.Name, it=>it);
                ;
            var str = "";
            foreach(var name in properties)
            {
                var val = props[name].GetValue(this).ToString();
                str += val;
            }
            return str;
                
        }
        [Benchmark]
        public string GetHostViaDictionary()
        {
            var props = this.ReadMyProperties();
            var str = "";
            foreach (var name in properties)
            {
                var val = props[name].ToString();
                str += val;
            }
            return str;
            
        }
        [Benchmark]
        public string GetHostViaSwitch()
        {
            var str = "";
            foreach (var name in properties)
            {
                var val = GetValueProperty(name).ToString();
                str += val;
            }
            return str;
            
        }
    }
}

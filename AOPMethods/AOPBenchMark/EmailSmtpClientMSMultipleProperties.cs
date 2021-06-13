using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace AOPBenchMark
{
    public partial class EmailSmtpClientMSMultipleProperties : EmailSmtpClientMS
    {
        
        [Benchmark]
        public IDictionary<string,object> GetHostReflection()
        {
            var props = this.GetType()
                .GetProperties()
                .Where(it=> it.CanWrite)
                .ToDictionary(it => it.Name, it=>it.GetValue(this));
                ;
            return props;
                
        }
        [Benchmark]
        public IDictionary<string, object> GetHostViaDictionary()
        {
            var props = this.ReadMyProperties();
            return props;
            
        }
        [Benchmark]
        public IDictionary<string, object> GetHostViaSwitch()
        {
            var props = ReadProperties
                .ToDictionary(it => it, it => GetValueProperty(it));
            return props;
            
            
        }
    }
}

using System;
using System.Diagnostics;

namespace AOPStatistics
{
    public struct Method
    {
        public string className;
        public string methodName;
        public int line;
    }
    public class AOPStatisticsMethod : IDisposable
    {
        private readonly string methodName;
        private readonly string className;
        private readonly int line;
        private readonly Stopwatch sw;
        public AOPStatisticsMethod(string className, string methodName, int line)
        {
            sw = Stopwatch.StartNew();            
            this.className = className;
            this.methodName = methodName;
            this.line = line;
        }

        
        public void Dispose()
        {
            sw.Stop();
            AOPStatistics.PushMethod(new Method() {  className = this.className, line= this.line, methodName= this.methodName}
                , sw.ElapsedMilliseconds);
            

        }
    }
}

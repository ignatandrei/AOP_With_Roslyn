using System;
using System.Diagnostics;

namespace AOPStatistics
{
    public class AOPMethod : IDisposable
    {
        private readonly string methodName;
        private readonly string className;
        private readonly int line;
        private readonly Stopwatch sw;
        public AOPMethod(string className, string methodName, int line)
        {
                   
            this.className = className;
            this.methodName = methodName;
            this.line = line;   
            sw = Stopwatch.StartNew();    
        }

        
        public void Dispose()
        {
            sw.Stop();
            GatherStatistics.PushMethod(new Method() {  className = this.className, line= this.line, methodName= this.methodName}
                , sw.ElapsedMilliseconds);
        }
    }
}

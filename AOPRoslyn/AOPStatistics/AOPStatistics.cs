using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOPStatistics
{
    public class AOPStatistics
    {
        public static ConcurrentDictionary<Method, long> timingMethod;
        static AOPStatistics()
        {
            timingMethod = new ConcurrentDictionary<Method, long>();
        }
        
        public static void PushMethod(Method m, long timeExecuting)
        {
            timingMethod.AddOrUpdate(m, timeExecuting, (newValue, oldValue) => oldValue + timeExecuting);
        }

        public static KeyValuePair<string,long>[] ProcessingClassTotalTime()
        {
            return timingMethod.GroupBy(it => it.Key.className)
                .Select(it => new KeyValuePair<string, long>(it.Key, it.Sum(a => a.Value)))
                .ToArray();
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOPStatistics
{
    public struct Data
    {
        public Method m;
        public int NumberHits;
        public long TotalDuration;
    }
    
    public class GatherStatistics
    {
        public static ConcurrentDictionary<Method, List<long>> timingMethod;
        static GatherStatistics()
        {
            timingMethod = new ConcurrentDictionary<Method, List<long>>();
        }
        
        public static void PushMethod(Method m, long timeExecuting)
        {
            var data = new List<long>();
            data.Add(timeExecuting);
            timingMethod.AddOrUpdate(m,data ,(newValue, oldValue) =>
                {
                    oldValue.Add(timeExecuting);
                    return oldValue;
                }
            );
        }
        public static Data[] DataGathered()
        {
            return GatherStatistics.timingMethod.Select(it =>
            new Data()
            {
                m = it.Key,
                NumberHits = it.Value.Count,
                TotalDuration = it.Value.Sum(),

            }).ToArray();
            
        }

    }
}

using AOPStatistics;
using ConsoleTables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestAOP
{
    [TestClass]
    public class TestInitializeAll
    {
        [AssemblyInitialize]
        public static void Init(TestContext testContext)
        {
            Console.WriteLine("test initialize");
            Console.WriteLine("ither clean!");
        }
        [AssemblyCleanup]
        public static void Clean()
        {
            
            var table = new ConsoleTable("Class", "Method(Line)", "NumberHit", "TotalTime", "AverageTime");
            if (GatherStatistics.timingMethod.Count == 0)
            {
                Console.WriteLine("no statistics loaded");
                return ;
            }
            var data = GatherStatistics.DataGathered()
                        .OrderByDescending(it => it.TotalDuration / it.NumberHits)
                        .ToArray();

            foreach (var item in data)
            {
                table.AddRow(item.m.className, $"{item.m.methodName}({item.m.line})", item.NumberHits, item.TotalDuration, (item.TotalDuration / item.NumberHits));
            }
            var s = table.ToMarkDownString();
            File.WriteAllText("statistics.txt", s);


        }
    }
}

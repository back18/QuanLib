using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib
{
    /// <summary>
    /// 性能分析工具
    /// </summary>
    public static class PerformanceAnalysis
    {
        public static TimeSpan GetRunTime(Action action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}

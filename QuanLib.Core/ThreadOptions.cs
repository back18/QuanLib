using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class ThreadOptions(string? name, bool isBackground, ThreadPriority priority)
    {
        public ThreadOptions() : this(null, true, ThreadPriority.Normal) { }

        public string? Name { get; set; } = name;

        public bool IsBackground { get; set; } = isBackground;

        public ThreadPriority Priority { get; set; } = priority;

        public ThreadOptions Clone()
        {
            return new(Name, IsBackground, Priority);
        }
    }
}

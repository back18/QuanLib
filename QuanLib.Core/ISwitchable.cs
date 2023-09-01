using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    /// <summary>
    /// 可开关
    /// </summary>
    public interface ISwitchable
    {
        public bool Runing { get; }

        public void Start();

        public void Stop();
    }
}

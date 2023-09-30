using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Events
{
    public class GuidEventArgs : EventArgs
    {
        public GuidEventArgs(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; }
    }
}

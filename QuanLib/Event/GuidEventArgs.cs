using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Event
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

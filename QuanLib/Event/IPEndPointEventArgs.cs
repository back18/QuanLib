using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Event
{
    public class IPEndPointEventArgs : EventArgs
    {
        public IPEndPointEventArgs(IPEndPoint iPEndPoint)
        {
            IPEndPoint = iPEndPoint;
        }

        public IPEndPoint IPEndPoint { get; }
    }
}

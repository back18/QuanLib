using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Event
{
    public class BytesEventArgs : EventArgs
    {
        public BytesEventArgs(byte[] bytes)
        {
            Bytes = bytes;
        }

        public byte[] Bytes { get; }
    }
}

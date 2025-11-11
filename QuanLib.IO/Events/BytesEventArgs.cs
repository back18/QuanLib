using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.IO.Events
{
    public class BytesEventArgs(byte[] buffer, int index, int length) : EventArgs
    {
        public byte[] Buffer { get; } = buffer;

        public int Index { get; } = index;

        public int Length { get; } = length;
    }
}

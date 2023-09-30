using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Events
{
    public class IndexEventArgs : EventArgs
    {
        public IndexEventArgs(int index)
        {
            Index = index;
        }

        public int Index { get; }
    }
}

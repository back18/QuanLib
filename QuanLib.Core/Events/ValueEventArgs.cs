using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Events
{
    public readonly struct ValueEventArgs : IValueEventArgs
    {
        public static readonly ValueEventArgs Empty = default;
    }
}

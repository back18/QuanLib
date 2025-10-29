using QuanLib.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public delegate void ValueEventHandler<TSender, TEventArgs>(TSender sender, TEventArgs e) where TEventArgs : IValueEventArgs;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface ISingleton<out TSelf> where TSelf : ISingleton<TSelf>
    {
        public static abstract bool IsLoaded { get; }

        public static abstract TSelf Instance { get; }
    }
}

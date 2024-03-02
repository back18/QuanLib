using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface ISingleton<TSelf, TArgs> where TSelf : ISingleton<TSelf, TArgs> where TArgs : InstantiateArgs
    {
        public static abstract bool IsInstanceLoaded { get; }

        public static abstract TSelf Instance { get; }

        public static abstract TSelf LoadInstance(TArgs args);
    }
}

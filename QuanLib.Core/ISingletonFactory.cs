using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLib.Core
{
    public interface ISingletonFactory<out TSelf> where TSelf : ISingletonFactory<TSelf>
    {
        public static abstract TSelf LoadInstance();
    }

    public interface ISingletonFactory<out TSelf, in TArgs> where TSelf : ISingletonFactory<TSelf, TArgs> where TArgs : InstantiateArgs
    {
        public static abstract TSelf LoadInstance(TArgs args);
    }
}

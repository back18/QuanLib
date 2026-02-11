using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface IDataModel<TSelf> : IValidatable where TSelf : IDataModel<TSelf>
    {
        public static abstract TSelf CreateDefault();
    }
}

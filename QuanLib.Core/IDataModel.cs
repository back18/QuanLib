using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface IDataModel<TSelf>
    {
        public static abstract TSelf CreateDefault();

        public static abstract void Validate(TSelf model, string name);
    }
}

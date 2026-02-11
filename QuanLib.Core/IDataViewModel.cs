using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface IDataViewModel<TSelf> where TSelf : IDataViewModel<TSelf>
    {
        public object ToDataModel();

        public static abstract TSelf FromDataModel(object model);
    }
}

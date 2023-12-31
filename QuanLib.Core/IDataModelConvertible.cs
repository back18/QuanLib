using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface IDataModelConvertible<TSelf, TModel>
    {
        public TModel ToDataModel();

        public static abstract TSelf FromDataModel(TModel model);

        public static abstract void ValidateDataModel(TModel model, string name);
    }
}

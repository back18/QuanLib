using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public interface IDataModelOwner<TSelf, TModel> where TSelf : IDataModelOwner<TSelf, TModel> where TModel : IDataModel<TModel>
    {
        public TModel ToDataModel();

        public static abstract TSelf FromDataModel(TModel model);
    }
}

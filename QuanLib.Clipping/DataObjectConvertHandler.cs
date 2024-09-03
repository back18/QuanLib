using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public class DataObjectConvertHandler
    {
        public DataObjectConvertHandler(MethodInfo convertMethod)
        {
            ArgumentNullException.ThrowIfNull(convertMethod, nameof(convertMethod));

            if (!convertMethod.GetParameters().Any() || !convertMethod.ReturnType.IsAssignableTo(typeof(IDataObject)))
                throw new ArgumentException("MethodInfo 不是合法的 Convert 方法", nameof(convertMethod));

            _convertMethod = convertMethod;
        }

        private readonly MethodInfo _convertMethod;

        public IDataObject Convert(IDataObject dataObject)
        {
            return _convertMethod.Invoke(dataObject, null) as IDataObject ?? throw new InvalidCastException();
        }
    }
}

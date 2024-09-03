using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public class DataObjectCreateHandler
    {
        public DataObjectCreateHandler(MethodInfo createMethod)
        {
            ArgumentNullException.ThrowIfNull(createMethod, nameof(createMethod));

            ParameterInfo[] parameters = createMethod.GetParameters();
            if (!createMethod.IsStatic ||
                parameters.Length != 1 ||
                parameters[0].ParameterType != typeof(object) ||
                !createMethod.ReturnType.IsAssignableTo(typeof(IDataObject)))
                throw new ArgumentException("MethodInfo 不是合法的 Create 方法", nameof(createMethod));

            _createMethod = createMethod;
        }

        private readonly MethodInfo _createMethod;

        public IDataObject Create(object data)
        {
            return _createMethod.Invoke(null, [data]) as IDataObject ?? throw new InvalidOperationException();
        }
    }
}

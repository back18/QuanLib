using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace QuanLib.Parsing
{
    public class ParseInvoker
    {
        public ParseInvoker(MethodInfo parseMethod)
        {
            ArgumentNullException.ThrowIfNull(parseMethod, nameof(parseMethod));

            if (!parseMethod.IsStatic )
                throw new ArgumentException("不支持实例方法", nameof(parseMethod));

            ParameterInfo[] parseParameters = parseMethod.GetParameters();

            if (parseParameters.Length < 1)
                throw new ArgumentException("方法参数数量与预期不符", nameof(parseMethod));

            if (parseParameters[0].ParameterType != typeof(string))
                throw new ArgumentException("方法签名与预期不符，首个参数应该为string", nameof(parseMethod));

            if (parseParameters.Length == 1)
                _hasFormat = false;
            else if (parseParameters.Length == 2 && parseParameters[1].ParameterType.IsAssignableTo(typeof(IFormatProvider)))
                _hasFormat = true;
            else
                throw new ArgumentException("方法签名与预期不符", nameof(parseMethod));

            _parseMethod = parseMethod;
        }

        private readonly MethodInfo _parseMethod;

        private readonly bool _hasFormat;

        public object Parse(string s, IFormatProvider? provider)
        {
            try
            {
                object?[] parameters = _hasFormat ? [s, provider] : [s];

                return _parseMethod.Invoke(null, parameters) ?? throw new NullReferenceException();
            }
            catch (TargetInvocationException ex) when (ex.InnerException is not null)
            {
                throw ex.InnerException;
            }
        }
    }
}

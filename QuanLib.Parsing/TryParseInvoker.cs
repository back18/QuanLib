using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace QuanLib.Parsing
{
    public class TryParseInvoker
    {
        public TryParseInvoker(MethodInfo tryParseMethod)
        {
            ArgumentNullException.ThrowIfNull(tryParseMethod, nameof(tryParseMethod));

            if (!tryParseMethod.IsStatic)
                throw new ArgumentException("不支持实例方法", nameof(tryParseMethod));

            ParameterInfo[] tryParseParameters = tryParseMethod.GetParameters();

            if (tryParseParameters.Length < 2)
                throw new ArgumentException("方法参数数量与预期不符", nameof(tryParseMethod));

            if (tryParseParameters[0].ParameterType != typeof(string))
                throw new ArgumentException("方法签名与预期不符，首个参数应该为string", nameof(tryParseMethod));

            if (!tryParseParameters[^1].IsOut)
                throw new ArgumentException("方法签名与预期不符，找不到out参数", nameof(tryParseMethod));

            if (tryParseParameters.Length == 2)
                _hasFormat = false;
            else if (tryParseParameters.Length == 3 && tryParseParameters[1].ParameterType.IsAssignableTo(typeof(IFormatProvider)))
                _hasFormat = true;
            else
                throw new ArgumentException("方法签名与预期不符", nameof(tryParseMethod));

            _tryParseMethod = tryParseMethod;
        }

        private readonly MethodInfo _tryParseMethod;

        private readonly bool _hasFormat;

        public bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out object result)
        {
            try
            {
                object?[] parameters = _hasFormat ? [s, provider, null] : [s, null];
                object? returnValue = _tryParseMethod.Invoke(null, parameters);
                result = parameters[^1];

                if (returnValue is true && result is not null)
                    return true;
                else
                    return false;
            }
            catch (TargetInvocationException ex) when (ex.InnerException is not null)
            {
                throw ex.InnerException;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.VisualObject
{
    public class MemberValue
    {
        public MemberValue(Type memberType, object? value, bool throwException)
        {
            ArgumentNullException.ThrowIfNull(memberType, nameof(memberType));

            MemberType = memberType;
            ValueType = value?.GetType() ?? memberType;
            Value = value;
            ThrownException = throwException;
        }

        public Type MemberType { get; }

        public Type ValueType { get; }

        public object? Value { get; }

        public bool ThrownException { get; }

        public static MemberValue GetValue(FieldInfo fieldInfo, object? obj)
        {
            ArgumentNullException.ThrowIfNull(fieldInfo, nameof(fieldInfo));

            object? value = fieldInfo.GetValue(obj);
            return new(fieldInfo.FieldType, value, false);
        }

        public static MemberValue GetValue(PropertyInfo propertyInfo, object? obj)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

            try
            {
                object? value = propertyInfo.GetValue(obj);
                return new(propertyInfo.PropertyType, value, false);
            }
            catch (Exception ex)
            {
                return new(propertyInfo.PropertyType, ex, true);
            }
        }

        public static MemberValue GetValue(MethodInfo methodInfo, object? obj, object?[]? parameters)
        {
            ArgumentNullException.ThrowIfNull(methodInfo, nameof(methodInfo));

            try
            {
                object? value = methodInfo.Invoke(obj, parameters);
                return new(methodInfo.ReturnType, value, false);
            }
            catch (Exception ex)
            {
                return new(methodInfo.ReturnType, ex, true);
            }
        }
    }
}

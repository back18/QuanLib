using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class SerializationMemberInfo
    {
        public SerializationMemberInfo(FieldInfo fieldInfo)
        {
            ArgumentNullException.ThrowIfNull(fieldInfo, nameof(fieldInfo));

            _fieldInfo = fieldInfo;
        }

        public SerializationMemberInfo(PropertyInfo propertyInfo)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));

            _propertyInfo = propertyInfo;
        }

        private readonly FieldInfo? _fieldInfo;

        private readonly PropertyInfo? _propertyInfo;

        public bool IsFieldInfo => _fieldInfo is not null;

        public bool IsProperty => _propertyInfo is not null;

        public FieldInfo AsFieldInfo() => _fieldInfo ?? throw new InvalidOperationException();

        public PropertyInfo AsPropertyInfo() => _propertyInfo ?? throw new InvalidOperationException();

        public MemberInfo MemberInfo
        {
            get
            {
                if (_fieldInfo is not null)
                    return _fieldInfo;
                else if (_propertyInfo is not null)
                    return _propertyInfo;
                else
                    throw new InvalidOperationException();
            }
        }

        public Type MemberType
        {
            get
            {
                if (_fieldInfo is not null)
                    return _fieldInfo.FieldType;
                else if (_propertyInfo is not null)
                    return _propertyInfo.PropertyType;
                else
                    throw new InvalidOperationException();
            }
        }

        public string MemberName => MemberInfo.Name;

        public object? GetValue(object? obj)
        {
            if (_fieldInfo is not null)
                return _fieldInfo.GetValue(obj);
            else if (_propertyInfo is not null)
                return _propertyInfo.GetValue(obj);
            else
                throw new InvalidOperationException();
        }

        public void SetValue(object? obj, object? value)
        {
            if (_fieldInfo is not null)
                _fieldInfo.SetValue(obj, value);
            else if (_propertyInfo is not null)
                _propertyInfo.SetValue(obj, value);
            else
                throw new InvalidOperationException();
        }
    }
}

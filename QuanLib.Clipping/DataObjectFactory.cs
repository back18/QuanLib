using QuanLib.Clipping.DataObjects;
using QuanLib.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Clipping
{
    public static class DataObjectFactory
    {
        private static readonly ConcurrentDictionary<Type, DataObjectCreateHandler> _createHandlers = [];

        private static readonly Type _dataObjectType = typeof(IDataObject);

        public static IDataObject Create(string format, object data)
        {
            ArgumentException.ThrowIfNullOrEmpty(format, nameof(format));
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            if (!FormatTypeManager.TryGetType(format, out var dataType))
                return new DataObject(format, data);

            if (!TryCreate(dataType, data, out var dataObject))
                return new DataObject(format, data);

            return dataObject;
        }

        public static IDataObject Create(Type dataType, object data)
        {
            ArgumentNullException.ThrowIfNull(dataType, nameof(dataType));
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            if (!_createHandlers.TryGetValue(dataType, out var createHandlers))
            {
                if (TryGetCreateHandler(dataType, out createHandlers))
                {
                    _createHandlers.TryAdd(dataType, createHandlers);
                }
                else
                {
                    throw new InvalidOperationException($"无法创建 {ObjectFormatter.Format(dataType)} 类型实例");
                }
            }

            return createHandlers.Create(data);
        }

        public static bool TryCreate(Type dataType, object data, [MaybeNullWhen(false)] out IDataObject result)
        {
            ArgumentNullException.ThrowIfNull(dataType, nameof(dataType));
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            if (!_createHandlers.TryGetValue(dataType, out var createHandlers))
            {
                if (TryGetCreateHandler(dataType, out createHandlers))
                {
                    _createHandlers.TryAdd(dataType, createHandlers);
                }
                else
                {
                    result = default;
                    return false;
                }
            }

            try
            {
                result = createHandlers.Create(data);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        private static bool TryGetCreateHandler(Type dataType, [MaybeNullWhen(false)] out DataObjectCreateHandler result)
        {
            ArgumentNullException.ThrowIfNull(dataType, nameof(dataType));

            if (!dataType.IsAssignableTo(_dataObjectType))
            {
                result = null;
                return false;
            }

            InterfaceMapping interfaceMapping = dataType.GetInterfaceMap(_dataObjectType);
            MethodInfo? createMethod =
                interfaceMapping.TargetMethods.Where(s => s.Name == "Create").FirstOrDefault() ??
                interfaceMapping.TargetMethods.Where(s => s.Name == "QuanLib.Clipping.IDataObject.Create").FirstOrDefault();

            if (createMethod is null)
            {
                result = null;
                return false;
            }

            result = new(createMethod);
            return true;
        }
    }
}

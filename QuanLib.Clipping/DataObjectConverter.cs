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
    public static class DataObjectConverter
    {
        private static readonly ConcurrentDictionary<TypeConvertInfo, DataObjectConvertHandler> _convertHandlers = [];

        private static readonly Type _dataObjectType = typeof(IDataObject);

        public static bool TryConvert<T>(IDataObject dataObject, [MaybeNullWhen(false)] out T result) where T : IDataObject
        {
            ArgumentNullException.ThrowIfNull(dataObject, nameof(dataObject));

            if (dataObject is T t)
            {
                result = t;
                return true;
            }

            if (dataObject is not IConvertible<T> convertible)
            {
                result = default;
                return false;
            }

            try
            {
                result = convertible.Convert();
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public static bool TryConvert(IDataObject dataObject, Type targetType, [MaybeNullWhen(false)] out IDataObject result)
        {
            ArgumentNullException.ThrowIfNull(dataObject, nameof(dataObject));
            ArgumentNullException.ThrowIfNull(targetType, nameof(targetType));

            Type sourceType = dataObject.GetType();
            if (sourceType.IsAssignableTo(targetType))
            {
                result = dataObject;
                return true;
            }

            TypeConvertInfo typeConvertInfo = new(sourceType, targetType);
            if (!_convertHandlers.TryGetValue(typeConvertInfo, out var convertHandler))
            {
                if (TryGetConvertHandler(typeConvertInfo, out convertHandler))
                {
                    _convertHandlers.TryAdd(typeConvertInfo, convertHandler);
                }
                else
                {
                    result = default;
                    return false;
                }
            }

            try
            {
                result = convertHandler.Convert(dataObject);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public static bool TryConvert(IDataObject dataObject, string targetFormat, [MaybeNullWhen(false)] out IDataObject result)
        {
            ArgumentNullException.ThrowIfNull(dataObject, nameof(dataObject));
            ArgumentException.ThrowIfNullOrEmpty(targetFormat, nameof(targetFormat));

            if (!FormatTypeManager.TryGetType(targetFormat, out var targetType))
            {
                result = default;
                return false;
            }

            return TryConvert(dataObject, targetType, out result);
        }

        private static bool TryGetConvertHandler(TypeConvertInfo typeConvertInfo, [MaybeNullWhen(false)] out DataObjectConvertHandler result)
        {
            Type sourceType = typeConvertInfo.SourceType;
            Type targetType = typeConvertInfo.TargetType;

            if (!sourceType.IsAssignableTo(_dataObjectType) ||
                !targetType.IsAssignableTo(_dataObjectType))
            {
                result = null;
                return false;
            }

            Type convertibleType = typeof(IConvertible<>).MakeGenericType(targetType);
            if (!sourceType.IsAssignableTo(convertibleType))
            {
                result = null;
                return false;
            }

            InterfaceMapping interfaceMapping = sourceType.GetInterfaceMap(convertibleType);
            MethodInfo? convertMethod =
                interfaceMapping.TargetMethods.Where(s => s.Name == "Convert").FirstOrDefault() ??
                interfaceMapping.TargetMethods.Where(s => s.Name == $"QuanLib.Clipping.IConvertible<{targetType.Namespace}.{targetType.Name}>.Convert").FirstOrDefault();

            if (convertMethod is null)
            {
                result = null;
                return false;
            }

            result = new(convertMethod);
            return true;
        }
    }
}

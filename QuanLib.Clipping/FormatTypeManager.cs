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
    public static class FormatTypeManager
    {
        static FormatTypeManager()
        {
            RegisterFormAssembly(Assembly.GetExecutingAssembly());
        }

        private static readonly ConcurrentDictionary<string, Type> _formatTypeMap = [];

        private static readonly Type _dataObjectType = typeof(IDataObject);

        public static ICollection<string> Formats => _formatTypeMap.Keys;

        public static void Register(string format, Type type)
        {
            ArgumentException.ThrowIfNullOrEmpty(format, nameof(format));
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            if (!type.IsAssignableTo(_dataObjectType))
                throw new ArgumentException("Type类型未实现 IDataObject 接口", nameof(type));

            if (_formatTypeMap.ContainsKey(format) || _formatTypeMap.Values.Where((w => w == type)).Any())
                throw new ArgumentException("已存在相同的键或值");

            _formatTypeMap.TryAdd(format, type);
        }

        public static void RegisterFormAssembly(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly, nameof(assembly));

            foreach (Type type in assembly.GetTypes())
            {
                DataObjectAttribute? dataObjectAttribute = type.GetCustomAttribute<DataObjectAttribute>();
                if (dataObjectAttribute is null)
                    continue;

                if (!type.IsAssignableTo(_dataObjectType))
                    continue;

                if (_formatTypeMap.ContainsKey(dataObjectAttribute.Format) || _formatTypeMap.Values.Where((w => w == type)).Any())
                    continue;

                _formatTypeMap.TryAdd(dataObjectAttribute.Format, type);
            }
        }

        public static bool TryGetType(string format, [MaybeNullWhen(false)] out Type type)
        {
            return _formatTypeMap.TryGetValue(format, out type);
        }
    }
}

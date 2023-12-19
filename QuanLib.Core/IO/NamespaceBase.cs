using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.IO
{
    public class NamespaceBase
    {
        public NamespaceBase(string @namespace)
        {
            ArgumentException.ThrowIfNullOrEmpty(@namespace, nameof(@namespace));

            FullName = @namespace;

            _items = new();
        }

        public string FullName { get; }

        private readonly Dictionary<string, NamespaceBase> _items;

        public NamespaceBase this[string name] => _items[name];

        public T AddNamespace<T>(string name) where T : NamespaceBase
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

            Type type = typeof(T);
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            foreach (ConstructorInfo constructor in constructors)
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
                {
                    object instance = constructor.Invoke(new object[] { Combine(name) });
                    T @namespace = (T)instance;
                    _items.Add(@namespace.FullName, @namespace);
                    return @namespace;
                }
            }

            throw new ArgumentException("找不到合适的构造函数");
        }

        public string Combine(string path)
        {
            ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));

            return Namespace.Combine(FullName, path);
        }
    }
}

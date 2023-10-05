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
    public class NamespaceManager : IReadOnlyDictionary<string, NamespaceManager>
    {
        public NamespaceManager(string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace))
                throw new ArgumentException($"“{nameof(@namespace)}”不能为 null 或空。", nameof(@namespace));

            FullName = @namespace;

            _items = new();
        }

        public string FullName { get; }

        private readonly Dictionary<string, NamespaceManager> _items;

        public NamespaceManager this[string key] => _items[key];

        public IEnumerable<string> Keys => _items.Keys;

        public IEnumerable<NamespaceManager> Values => _items.Values;

        public int Count => _items.Count;

        public T AddNamespace<T>(string name) where T : NamespaceManager
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));

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
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException($"“{nameof(path)}”不能为 null 或空。", nameof(path));

            return Namespace.Combine(FullName, path);
        }

        public bool ContainsKey(string key)
        {
            return _items.ContainsKey(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out NamespaceManager value)
        {
            return _items.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, NamespaceManager>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }
    }
}

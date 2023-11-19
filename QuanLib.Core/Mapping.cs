using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class Mapping<TKey, TValue> where TKey : notnull where TValue : notnull
    {
        public Mapping()
        {
            _keyMap = [];
            _valueMap = [];
        }

        private readonly Dictionary<TKey, TValue> _keyMap;

        private readonly Dictionary<TValue, TKey> _valueMap;

        public IReadOnlyDictionary<TKey, TValue> KeyMap => _keyMap;

        public IReadOnlyDictionary<TValue, TKey> ValueMap => _valueMap;

        public TValue this[TKey key]
        {
            get => KeyMap[key];
            set
            {
                _valueMap.Remove(_keyMap[key]);
                _valueMap.Add(value, key);
                _keyMap[key] = value;
            }
        }

        public TKey this[TValue value_]
        {
            get => ValueMap[value_];
            set
            {
                _keyMap.Remove(_valueMap[value_]);
                _keyMap.Add(value, value_);
                _valueMap[value_] = value;
            }
        }

        public void AddKey(TKey key, TValue value)
        {
            _keyMap.Add(key, value);
            _valueMap.Add(value, key);
        }

        public void AddValue(TValue value, TKey key)
        {
            _valueMap.Add(value, key);
            _keyMap.Add(key, value);
        }

        public bool TryAddKey(TKey key, TValue value)
        {
            return _keyMap.TryAdd(key, value) && _valueMap.TryAdd(value, key);
        }

        public bool TryAddValue(TValue value, TKey key)
        {
            return _valueMap.TryAdd(value, key) && _keyMap.TryAdd(key, value);
        }

        public bool RemoveKey(TKey key)
        {
            return _valueMap.Remove(_keyMap[key]) && _keyMap.Remove(key);

        }

        public bool RemoveValue(TValue value)
        {
            return _keyMap.Remove(_valueMap[value]) && _valueMap.Remove(value);
        }

        public bool ContainsKey(TKey key)
        {
            return _keyMap.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _valueMap.ContainsKey(value);
        }

        public bool TryGetKey(TValue value, [MaybeNullWhen(false)] out TKey key)
        {
            return _valueMap.TryGetValue(value, out key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return _keyMap.TryGetValue(key, out value);
        }
    }
}

using QuanLib.Core.ExceptionHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public class CacheDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
    {
        public CacheDictionary(TimeSpan expirationTime)
        {
            ExpirationTime = expirationTime;
            _items = new();
            _timestamps = new();
        }

        private readonly Dictionary<TKey, DateTime> _timestamps;

        private readonly Dictionary<TKey, TValue> _items;

        public TValue this[TKey key]
        {
            get
            {
                if (DateTime.Now - _timestamps[key] >= ExpirationTime)
                {
                    _timestamps.Remove(key);
                    _items.Remove(key);
                }

                return _items[key];
            }
            set
            {
                _timestamps[key] = DateTime.Now;
                _items[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                ClearExpiredItems();
                return _items.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                ClearExpiredItems();
                return _items.Values;
            }
        }

        public int Count
        {
            get
            {
                ClearExpiredItems();
                return _items.Count;
            }
        }

        public bool IsReadOnly => false;

        public TimeSpan ExpirationTime { get; }

        public void ClearExpiredItems()
        {
            DateTime now = DateTime.Now;
            var expiredKeys = _timestamps.Where(kv => now - kv.Value >= ExpirationTime).Select(kv => kv.Key);

            foreach (var key in expiredKeys)
            {
                _timestamps.Remove(key);
                _items.Remove(key);
            }
        }

        public void Add(TKey key, TValue value)
        {
            _timestamps.Add(key, DateTime.Now);
            _items.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            _timestamps.Remove(key);
            return _items.Remove(key);
        }

        public void Clear()
        {
            _timestamps.Clear();
            _items.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            if (!_timestamps.TryGetValue(key, out var time))
                return false;

            if (DateTime.Now - time >= ExpirationTime)
            {
                _timestamps.Remove(key);
                _items.Remove(key);
                return false;
            }

            return true;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (!_timestamps.TryGetValue(key, out var time))
            {
                value = default;
                return false;
            }

            if (DateTime.Now - time >= ExpirationTime)
            {
                _timestamps.Remove(key);
                _items.Remove(key);
                value = default;
                return false;
            }

            return _items.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            ClearExpiredItems();
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            ClearExpiredItems();
            return ((IEnumerable)_items).GetEnumerator();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }
    }
}

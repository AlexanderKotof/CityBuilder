using System;
using System.Collections;
using System.Collections.Generic;

namespace CityBuilder.Reactive
{
    public class ReactiveDictionary<TKey, TValue> : IDisposable, IReadOnlyDictionary<TKey, TValue>
    {
        public int Count => _innerDictionary.Count;
        public TValue this[TKey key] => _innerDictionary[key];
        public IEnumerable<TKey> Keys => _innerDictionary.Keys;
        public IEnumerable<TValue> Values => _innerDictionary.Values;
        
        private readonly Dictionary<TKey, TValue> _innerDictionary = new();
        
        private Action<TKey, TValue> _onAdd;
        private Action<TKey, TValue> _onRemove;
        
        public void SubscribeAdd(Action<TKey, TValue> listener)
        {
            _onAdd += listener;
        }
        public void UnsubscribeAdd(Action<TKey, TValue> listener)
        {
            _onAdd -= listener;
        }
        public void SubscribeRemove(Action<TKey, TValue> listener)
        {
            _onRemove += listener;
        }
        public void UnsubscribeRemove(Action<TKey, TValue> listener)
        {
            _onRemove -= listener;
        }

        public void Add(TKey key, TValue value)
        {
            _innerDictionary.Add(key, value);
            _onAdd?.Invoke(key, value);
        }

        public void Remove(TKey key)
        {
            if (_innerDictionary.Remove(key, out TValue value))
            {
                _onRemove?.Invoke(key, value);
            }
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            if (_innerDictionary.ContainsKey(key))
            {
                Remove(key);
            }
            
            Add(key, value);
        }
        
        public void Dispose()
        {
            _onAdd = null;
            _onRemove = null;
            _innerDictionary.Clear();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public bool ContainsKey(TKey key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            _innerDictionary.Clear();
        }
    }
}
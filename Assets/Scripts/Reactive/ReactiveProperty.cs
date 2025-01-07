using System;
using System.Collections;
using System.Collections.Generic;

namespace SityBuilder.Reactive
{
    public class ReactiveProperty<T> : IDisposable
    {
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        private T? _value;

        private event Action<T> OnValueChanged;

        public ReactiveProperty()
        {
            _value = default!;
        }

        public ReactiveProperty(T value)
        {
            _value = value;
        }

        public void Set(T value)
        {
            Value = value;
        }

        public void AddListener(Action<T> listener)
        {
            OnValueChanged += listener;
        }

        public void RemoveListener(Action<T> listener)
        {
            OnValueChanged -= listener;
        }

        public void Dispose()
        {
            OnValueChanged = null;
        }

        public static implicit operator T(ReactiveProperty<T> property) => property._value;

        public bool HasValue()
        {
            return _value != null;
        }
    }

    public class ReactiveCollection<T> : IDisposable, IReadOnlyCollection<T>
    {
        public int Count => _innerCollection.Count;
        
        private readonly List<T> _innerCollection = new();
        
        private Action<T> _onAdd;
        private Action<T> _onRemove;
        
        public void Dispose()
        {
            _onAdd = null;
            _onRemove = null;
            _innerCollection.Clear();
        }
        public void SubscribeAdd(Action<T> listener)
        {
            _onAdd += listener;
        }
        public void UnsubscribeAdd(Action<T> listener)
        {
            _onAdd -= listener;
        }
        public void SubscribeRemove(Action<T> listener)
        {
            _onRemove += listener;
        }
        public void UnsubscribeRemove(Action<T> listener)
        {
            _onRemove -= listener;
        }

        public void Add(T item)
        {
            _innerCollection.Add(item);
            _onAdd?.Invoke(item);
        }
        
        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }
        
        public void Remove(T item)
        {
            if (_innerCollection.Remove(item))
            {
                _onRemove?.Invoke(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

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
    }
}
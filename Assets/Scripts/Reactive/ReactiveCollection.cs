using System;
using System.Collections;
using System.Collections.Generic;

namespace CityBuilder.Reactive
{
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
        
        public bool Remove(T item)
        {
            if (_innerCollection.Remove(item))
            {
                _onRemove?.Invoke(item);
                return true;
            }
            
            return false;
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
}
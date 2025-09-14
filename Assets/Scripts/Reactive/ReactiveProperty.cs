using System;

namespace CityBuilder.Reactive
{
    public interface IReadonlyReactiveProperty<T>
    {
        T Value { get; }
        void AddListener(Action<T> listener);
        void RemoveListener(Action<T> listener);
    }
    
    public class ReactiveProperty<T> : IReadonlyReactiveProperty<T>, IDisposable
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

        public override string ToString()
        {
            return _value?.ToString();
        }
    }
}
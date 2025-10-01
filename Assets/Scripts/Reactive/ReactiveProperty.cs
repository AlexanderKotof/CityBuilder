using System;

namespace CityBuilder.Reactive
{
    public interface IReadonlyReactiveProperty<out T>
    {
        T? Value { get; }
        void AddListener(Action<T?> listener);
        void RemoveListener(Action<T?> listener);
    }
    
    public class ReactiveProperty<T> : IReadonlyReactiveProperty<T>, IDisposable
    {
        public T? Value
        {
            get => _value;
            set
            {
                _value = value;
                _onValueChanged?.Invoke(_value);
            }
        }

        private T? _value;

        private Action<T?>? _onValueChanged = delegate { };

        public ReactiveProperty()
        {
            _value = default!;
        }

        public ReactiveProperty(T? value)
        {
            _value = value;
        }

        public void Set(T? value)
        {
            Value = value;
        }

        public void AddListener(Action<T?> listener)
        {
            _onValueChanged += listener;
        }

        public void RemoveListener(Action<T?> listener)
        {
            _onValueChanged -= listener;
        }

        public void Dispose()
        {
            _onValueChanged = null;
        }

        public static implicit operator T(ReactiveProperty<T> property) => property._value ?? default!;

        public bool HasValue() => _value != null;

        public override string ToString() => _value?.ToString() ?? "null";
    }
}
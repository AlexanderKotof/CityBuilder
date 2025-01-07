using System;
using System.Collections.Generic;

public class Dependencies
{
    private readonly Dictionary<Type, object> _container = new Dictionary<Type, object>();
    public Dependencies() { }

    public void Register<T>(T value) => _container.Add(typeof(T), value);

    public T Resolve<T>()
    {
        if (_container.TryGetValue(typeof(T), out var value))
        {
            return (T)value;
        }

        throw new ArgumentException($"Unable to resolve {typeof(T)}");
    }
}
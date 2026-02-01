using System;
using System.Collections.Generic;

namespace CityBuilder.Dependencies
{
    /// <summary>
    /// [Obsolete] Устарело - пока еще используется в паре мест
    /// </summary>
    public class DependencyContainer : IDependencyContainer
    {
        private readonly Dictionary<Type, object> _container = new Dictionary<Type, object>();
        
        public void Register<T>(T value) => _container.Add(typeof(T), value);
        
        public void Register(Type type, object value) => _container.Add(type, value);

        public T Resolve<T>()
        {
            if (_container.TryGetValue(typeof(T), out var value))
            {
                return (T)value;
            }

            throw new ArgumentException($"Unable to resolve {typeof(T)}");
        }
    }
}
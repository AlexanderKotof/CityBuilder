using System;

namespace CityBuilder.Dependencies
{
    public interface IDependencyContainer
    {
        void Register<T>(T value);
        void Register(Type type, object value);
        T Resolve<T>();
    }
}
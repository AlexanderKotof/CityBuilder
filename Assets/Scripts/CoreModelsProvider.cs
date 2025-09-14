using System;
using System.Collections.Generic;

public class CoreModelsProvider : ICoreModelsProvider
{
    private readonly Dictionary<Type, object> _models = new();
    public T GetModel<T>() where T : class
    {
        return (T)_models.GetValueOrDefault(typeof(T));
    }

    public T RegisterModel<T>(T instance) where T : class
    {
        return _models.TryAdd(instance.GetType(), instance) ? instance : GetModel<T>();
    }
}
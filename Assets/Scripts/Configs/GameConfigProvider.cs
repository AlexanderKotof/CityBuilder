using System;
using System.Collections.Generic;
using Configs.Schemes;

namespace Configs
{
    public class GameConfigProvider
    {
        public IReadOnlyDictionary<Type, IGameConfigScheme> Map => _configsMap;
        
        private readonly Dictionary<Type, IGameConfigScheme> _configsMap = new();

        public void Register(IGameConfigScheme scheme, Type type)
        {
            _configsMap.Add(type, scheme);
        }
        
        public T GetConfig<T>() where T : IGameConfigScheme
        {
            return (T)_configsMap[typeof(T)];
        }
    }
}
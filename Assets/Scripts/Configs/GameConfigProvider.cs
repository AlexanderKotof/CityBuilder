using System;
using System.Collections.Generic;

namespace Configs
{
    public class GameConfigProvider
    {
        private readonly Dictionary<Type, IGameConfig> _configsMap = new();
        
        public GameConfigProvider(IEnumerable<IGameConfig> configs)
        {
            foreach (var gameConfigPiece in configs)
            {
                _configsMap.Add(configs.GetType(), gameConfigPiece);
            }
        }

        public void Register(IGameConfig scheme, Type type)
        {
            _configsMap.Add(type, scheme);
        }
        
        public T GetConfig<T>() where T : IGameConfig
        {
            return (T)_configsMap[typeof(T)];
        }
    }
}
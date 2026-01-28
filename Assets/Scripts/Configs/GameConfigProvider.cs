using System;
using System.Collections.Generic;
using Configs.Scriptable;

namespace Configs
{
    public class GameConfigProvider
    {
        private readonly Dictionary<Type, IConfigBase> _configsMap = new();

        public GameConfigProvider() { }
        public GameConfigProvider(IConfigBase[] configs)
        {
            foreach (var gameConfigPiece in configs)
            {
                _configsMap.Add(configs.GetType(), gameConfigPiece);
            }
        }

        public void Register(IConfigBase scheme, Type type)
        {
            _configsMap.Add(type, scheme);
        }
        
        public T GetConfig<T>() where T : IConfigBase
        {
            return (T)_configsMap[typeof(T)];
        }
    }

    public interface IConfigBase
    {
    }
}
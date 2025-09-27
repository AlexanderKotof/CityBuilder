using System;
using System.Collections.Generic;
using System.Linq;
using Configs.Schemes;

namespace Configs.Utilities
{
    public static class ConfigTypeUtility
    {
        public static IEnumerable<Type> GetAllConfigTypes()
        {
            var configTypes = typeof(IGameConfigScheme).Assembly.GetTypes().
                Where(type => typeof(IGameConfigScheme).IsAssignableFrom(type) && type.IsAbstract == false && type.IsInterface == false);
            return configTypes;
        }
    }
}
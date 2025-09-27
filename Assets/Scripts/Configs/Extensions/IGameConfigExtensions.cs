using System;

namespace Configs.Extensions
{
    public static class IGameConfigExtensions
    {
        public static string GetConfigFileName(this Type gameConfig)
        {
            return gameConfig.Name + ".json";
        }
    }
}
using System;

namespace Configs.Schemes
{
    public static class IGameConfigExtensions
    {
        public static string GetConfigFileName(this Type gameConfig)
        {
            return gameConfig.Name + ".json";
        }
    }
}
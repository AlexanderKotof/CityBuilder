using Configs.Implementation.Buildings;
using Configs.Schemes;
using Configs.Scriptable;

namespace Configs.Extensions
{
    public static class GameConfigProviderExtensions
    {
        public static BuildingsSettingsSO BuildingsSettings(this GameConfigProvider provider) =>
            provider.GetConfig<BuildingsSettingsSO>();
        
        public static CommonGameSettingsSO CommonGameSettings(this GameConfigProvider provider) =>
            provider.GetConfig<CommonGameSettingsSO>();
        
        
    }
}
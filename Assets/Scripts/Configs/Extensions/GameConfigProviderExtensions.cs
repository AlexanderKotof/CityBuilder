using Configs.Schemes;

namespace Configs.Extensions
{
    public static class GameConfigProviderExtensions
    {
        public static BuildingsSettingsScheme BuildingsSettings(this GameConfigProvider provider) =>
            provider.GetConfig<BuildingsSettingsScheme>();
        
        public static CommonGameSettings CommonGameSettings(this GameConfigProvider provider) =>
            provider.GetConfig<CommonGameSettings>();
    }
}
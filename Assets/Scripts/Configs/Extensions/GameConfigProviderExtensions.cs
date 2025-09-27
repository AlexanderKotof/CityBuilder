namespace Configs.Schemes
{
    public static class GameConfigProviderExtensions
    {
        public static BuildingsSettings BuildingsSettings(this GameConfigProvider provider) =>
            provider.GetConfig<BuildingsSettings>();
        
        public static CommonGameSettings CommonGameSettings(this GameConfigProvider provider) =>
            provider.GetConfig<CommonGameSettings>();
    }
}
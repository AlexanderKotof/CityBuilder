using Configs;
using Configs.Scriptable;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class GameConfigsInstaller : LifetimeScope
    {
        public CommonGameSettingsSO CommonGameSettingsSO;
        public BuildingsSettingsSO BuildingsSettingsSO;
        public ResourcesDefaultConfigurationSO ResourcesDefaultConfigurationSO;
        public BattleUnitsConfigSO BattleUnitsConfigSO;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(CommonGameSettingsSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(BuildingsSettingsSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(ResourcesDefaultConfigurationSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(BattleUnitsConfigSO).AsSelf().As<IGameConfig>();

            builder.Register<GameConfigProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}
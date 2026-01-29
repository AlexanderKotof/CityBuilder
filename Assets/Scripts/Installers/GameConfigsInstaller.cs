using Configs;
using Configs.Scriptable;
using Configs.Scriptable.Battle;
using Configs.Scriptable.Buildings;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class GameConfigsInstaller : LifetimeScope
    {
        public CommonGameSettingsSo CommonGameSettingsSO;
        public BuildingsSettingsSo BuildingsSettingsSO;
        public ResourcesDefaultConfigurationSo ResourcesDefaultConfigurationSO;
        public BattleUnitsConfigSO BattleUnitsConfigSO;
        public MergeFeatureConfigurationSo MergeFeatureConfigurationSO;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(CommonGameSettingsSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(BuildingsSettingsSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(ResourcesDefaultConfigurationSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(BattleUnitsConfigSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(MergeFeatureConfigurationSO).AsSelf().As<IGameConfig>();

            builder.Register<GameConfigProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}
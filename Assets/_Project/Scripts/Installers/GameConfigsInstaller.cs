using CityBuilder.Configs;
using CityBuilder.Configs.Scriptable;
using CityBuilder.Configs.Scriptable.Battle;
using CityBuilder.Configs.Scriptable.Buildings;
using CityBuilder.Configs.Scriptable.Buildings.Merge;
using VContainer;
using VContainer.Unity;

namespace CityBuilder.Installers
{
    public class GameConfigsInstaller : LifetimeScope
    {
        public CommonGameSettingsSo CommonGameSettingsSO;
        public InteractionSettingsSo InteractionSettingsSO;
        public BuildingsSettingsSo BuildingsSettingsSO;
        public ResourcesDefaultConfigurationSo ResourcesDefaultConfigurationSO;
        public BattleUnitsConfigSO BattleUnitsConfigSO;
        public MergeFeatureConfigurationSo MergeFeatureConfigurationSO;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(CommonGameSettingsSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(InteractionSettingsSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(BuildingsSettingsSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(ResourcesDefaultConfigurationSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(BattleUnitsConfigSO).AsSelf().As<IGameConfig>();
            builder.RegisterInstance(MergeFeatureConfigurationSO).AsSelf().As<IGameConfig>();

            builder.Register<GameConfigProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}
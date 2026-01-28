using Configs;
using Configs.Schemes;
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
            builder.RegisterInstance(CommonGameSettingsSO).As<CommonGameSettingsSO>().As<IConfigBase>();
            builder.RegisterInstance(BuildingsSettingsSO).As<BuildingsSettingsSO>().As<IConfigBase>();
            builder.RegisterInstance(ResourcesDefaultConfigurationSO).As<ResourcesDefaultConfigurationSO>().As<IConfigBase>();
            builder.RegisterInstance(BattleUnitsConfigSO).As<BattleUnitsConfigSO>().As<IConfigBase>();

            builder.Register<GameConfigProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}
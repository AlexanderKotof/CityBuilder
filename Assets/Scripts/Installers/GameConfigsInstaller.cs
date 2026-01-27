using System;
using System.Collections.Generic;
using Configs.Scriptable;
using GameSystems.Common.GameConfigs;
using GameSystems.Implementation;
using GameSystems.Implementation.BattleSystem;
using GameSystems.Implementation.BuildingsFeature;
using GameSystems.Implementation.CheatsFeature;
using GameSystems.Implementation.GameInteractionFeature;
using GameSystems.Implementation.GameTimeSystem;
using GameSystems.Implementation.PopulationFeature;
using GameSystems.Implementation.ProducingFeature;
using GameSystems.Implementation.ResourcesStorageFeature;
using UnityEngine;
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
            builder.RegisterInstance(CommonGameSettingsSO).As<CommonGameSettingsSO>();
            builder.RegisterInstance(BuildingsSettingsSO).As<BuildingsSettingsSO>();
            builder.RegisterInstance(ResourcesDefaultConfigurationSO).As<ResourcesDefaultConfigurationSO>();
            builder.RegisterInstance(BattleUnitsConfigSO).As<BattleUnitsConfigSO>();
        }
    }

    public class GameSystemsInstaller : LifetimeScope
    {
        public static readonly HashSet<Type> CommonSystems = new()
        {
            typeof(ViewsSystem),
            typeof(WindowSystem),
            typeof(PlayerInputSystem),
        };
        
        public static readonly HashSet<Type> GamePlayFeatures = new()
        {
            typeof(GameTimeSystem),
            typeof(ResourcesFeature),
            typeof(CellGridFeature),
            typeof(GameInteractionFeature),
            typeof(BuildingFeature),
            typeof(ResourcesProductionFeature),
            typeof(PopulationFeature),
            typeof(ResourcesStorageFeature),
            typeof(BattleFeature),
            typeof(CheatsFeature),
        };
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("Begin common systems registration");
            foreach (var system in CommonSystems)
            {
                builder.Register(system, Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            }
            
            Debug.Log("Begin GamePlayFeatures registration");
            foreach (var system in GamePlayFeatures)
            {
                builder.Register(system, Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            }
        }
    }
}
using System;
using System.Collections.Generic;
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

namespace GameSystems
{
    public static class GameSystemsSets
    {
        public static readonly HashSet<Type> CommonSystems = new()
        {
            typeof(GameConfigInitializationSystem),
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
    }
}
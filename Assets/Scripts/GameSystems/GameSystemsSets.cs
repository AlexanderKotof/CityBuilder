using System;
using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using Configs;
using GameSystems.Implementation.BattleSystem;
using GameSystems.Implementation.CheatsFeature;
using GameSystems.Implementation.PopulationFeature;
using ResourcesSystem;

namespace GameSystems
{
    public static class GameSystemsSets
    {
        public static readonly HashSet<Type> CommonSystems = new()
        {
            typeof(GameConfigInitializationSystem),
        };
        
        public static readonly HashSet<Type> GamePlayFeatures = new()
        {
            typeof(GameTimeSystem.GameTimeSystem),
            typeof(ResourcesFeature),
            typeof(CellGridFeature),
            typeof(GameInteractionFeature),
            typeof(BuildingFeature),
            typeof(ProducingFeature.ResourcesProductionFeature),
            typeof(PopulationFeature),
            typeof(ResourcesStorageFeature),
            typeof(BattleFeature),
            typeof(CheatsFeature),
        };
    }
}
using System;
using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using GameSystems.Implementation.CheatsFeature;
using GameSystems.Implementation.PopulationFeature;
using ResourcesSystem;

namespace GameSystems
{
    public static class GameSystemsSet
    {
        public static readonly HashSet<Type> GameSystemTypes = new()
        {
            typeof(GameTimeSystem.GameTimeSystem),
            typeof(CellGridFeature),
            typeof(ProducingFeature.ResourcesProductionFeature),
            typeof(PopulationFeature),
            typeof(ResourcesStorageFeature),
            typeof(BuildingViewsFeature),
            typeof(CheatsFeature),
        };
    }
}
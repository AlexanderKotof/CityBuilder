using System.Collections.Generic;
using CityBuilder.Configs.Implementation.Common;
using CityBuilder.Configs.Scriptable.Buildings.Functions;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.ResourcesFeature.ProducingFeature
{
    public record BuildingResourceProductionUnit(BuildingModel Building, ResourceProductionBuildingFunctionSo Function) : IResourceProducer
    {
        private ResourceProductionBuildingFunctionSo Function { get; } = Function;
        private BuildingModel Building { get; } = Building;

        private BuildingProductionByLevel CurrentLevelProduction =>
            Function.ProductionsByBuildingLevel[
                Mathf.Min(Building.Level.Value, Function.ProductionsByBuildingLevel.Length - 1)];

        public bool CanProduce()
        {
            return true;
        }

        public IEnumerable<ResourceConfig> GetCosts()
        {
            return CurrentLevelProduction.RequireResourcesForProduction;
        }

        public IEnumerable<ResourceConfig> GetProduction()
        {
            return CurrentLevelProduction.ProduceResourcesByTick;
        }
    }
}
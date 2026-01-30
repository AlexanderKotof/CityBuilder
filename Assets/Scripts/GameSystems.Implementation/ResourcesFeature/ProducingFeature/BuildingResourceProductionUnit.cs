using System.Collections.Generic;
using Configs.Implementation.Common;
using Configs.Scriptable.Buildings.Functions;
using GameSystems.Implementation.BuildingSystem.Domain;
using UnityEngine;

namespace GameSystems.Implementation.ProducingFeature
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
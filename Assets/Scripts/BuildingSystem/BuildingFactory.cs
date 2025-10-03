using System.Collections;
using CityBuilder.Grid;
using Configs.Implementation.Buildings;
using Configs.Schemes;
using UnityEngine;
using ViewSystem;

namespace CityBuilder.BuildingSystem
{
    public class BuildingFactory
    {
        public BuildingModel Create(BuildingConfigScheme config, CellModel cellModel)
        {
            var building = new BuildingModel(1, config);
            return building;
        }
    }
}

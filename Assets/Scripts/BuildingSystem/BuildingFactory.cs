using System.Collections;
using CityBuilder.Grid;
using UnityEngine;
using ViewSystem;

namespace CityBuilder.BuildingSystem
{
    public class BuildingFactory
    {
        public BuildingModel Create(BuildingConfig config, CellModel cellModel)
        {
            var building = new BuildingModel(1, config);
            return building;
        }
    }
}

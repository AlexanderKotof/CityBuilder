using System.Collections;
using CityBuilder.Grid;
using UnityEngine;
using ViewSystem;

namespace CityBuilder.BuildingSystem
{
    public class BuildingFactory
    {
        private readonly ViewsProvider _viewsProvider;

        public BuildingFactory(ViewsProvider viewsProvider)
        {
            _viewsProvider = viewsProvider;
        }
        
        public BuildingModel Create(BuildingConfig config, CellModel cellModel)
        {
            var building = new BuildingModel(1, config);
            return building;
        }
    }
}

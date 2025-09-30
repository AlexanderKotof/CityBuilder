using System.Collections;
using CityBuilder.Grid;
using UnityEngine;
using ViewSystem;

namespace CityBuilder.BuildingSystem
{
    public class BuildingFactory
    {
        private readonly IViewsProvider _viewsProvider;

        public BuildingFactory(IViewsProvider viewsProvider)
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

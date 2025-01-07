using System.Collections;
using UnityEngine;
using ViewSystem;

namespace BuildingSystem
{
    public class BuildingFactory
    {
        private readonly ViewsProvider _viewsProvider;

        public BuildingFactory(ViewsProvider viewsProvider)
        {
            _viewsProvider = viewsProvider;
        }
        
        public Building Create(BuildingConfig config, CellModel cellModel)
        {
            var building = new Building(1, config);
            return building;
        }
    }
}

using CityBuilder.Grid;
using UnityEngine;
using VContainer.Unity;

namespace GameSystems.Implementation
{
    public class CellGridFeature : IInitializable
    {
        public GridManager GridManager { get; private set; }

        public CellGridFeature(GridManager gridManager)
        {
            GridManager = gridManager;
        }

        public void Initialize()
        {
            RegisterGrids();
        }
        
        private void RegisterGrids()
        {
            var grids = GameObject.FindObjectsOfType<GridComponent>();
            foreach (var grid in grids)
            {
                GridManager.RegisterGrid(grid);
            }
        }
    }
}
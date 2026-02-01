using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.CellGridFeature
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
using System.Threading.Tasks;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using UnityEngine;

namespace GameSystems.Implementation
{
    public class CellGridFeature : GameSystemBase
    {
        public GridManager GridManager { get; private set; }

        public CellGridFeature(IDependencyContainer container) : base(container)
        {
            GridManager = new GridManager();
        }

        public override Task Init()
        {
            RegisterGrids();
            return Task.CompletedTask;
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
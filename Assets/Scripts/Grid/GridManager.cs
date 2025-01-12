using System;
using System.Collections.Generic;

namespace CityBuilder.Grid
{
    public class GridManager
    {
        private readonly Dictionary<IGridComponent, GridModel> _gridModels = new Dictionary<IGridComponent, GridModel> ();
        public event Action<GridModel> OnGridAdded;
        public IReadOnlyCollection<GridModel> GridModels => _gridModels.Values;

        public void RegisterGrid(IGridComponent gridComponent)
        {
            var gridModel = new GridModel(gridComponent);
            _gridModels.Add(gridComponent, gridModel);
            OnGridAdded?.Invoke(gridModel);
        }

        public bool TryGetGridModel(IGridComponent gridComponent, out GridModel gridModel)
        {
            return _gridModels.TryGetValue(gridComponent, out gridModel);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilder.Grid
{
    public class GridModel : IEquatable<GridModel>
    {
        private readonly Dictionary<GridPosition, CellModel> _grid = new();
        public Transform Transform { get; }
        
        public Vector2Int Size { get; private set; }
    
        private readonly Guid _id = Guid.NewGuid();
        private IGridComponent _view;

        public GridModel(IGridComponent gridComponent) : this(gridComponent.Size.x, gridComponent.Size.y)
        {
            _view = gridComponent;
            Transform = gridComponent.Transform;
        }
    
        private GridModel(int width, int length)
        {
            Size = new Vector2Int(width, length);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    var position = new GridPosition(x, y);
                    var cell = new CellModel(position, this);
                    _grid.Add(position, cell);
                }
            }
        }

        public CellModel GetCell(GridPosition position)
        {
            return _grid[position];
        }

        public CellModel GetCell(int x, int y)
        {
            return _grid[new GridPosition(x, y)];
        }

        public bool TryGetCell(Vector2Int position, out CellModel cellModel)
        {
            return TryGetCell(new GridPosition(position), out cellModel);
        }

        public bool TryGetCell(GridPosition position, out CellModel cellModel)
        {
            return _grid.TryGetValue(position, out cellModel);
        }
    
        public Vector3 GridPositionToCellWorldPosition(GridPosition position)
        {
            return GetCellWorldPosition(position.Value);
        }
    
        private Vector3 GetCellWorldPosition(Vector2 position)
        {
            Vector3 hitPosition2d = new Vector3(
                Mathf.FloorToInt(position.x),
                0,
                Mathf.FloorToInt(position.y));

            return Transform.TransformPoint(hitPosition2d);
        }

        public bool Equals(GridModel? other)
        {
            return other != null && _id.Equals(other._id);
        }
    }
}
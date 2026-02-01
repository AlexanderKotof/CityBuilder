using System;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.CellGridFeature.Grid
{
    public struct GridPosition : IEquatable<GridPosition>
    {
        public Vector2Int Value { get; set; }
        public int X => Value.x;
        public int Y => Value.y;

        public GridPosition(int x, int y)
        {
            Value = new Vector2Int(x, y);
        }

        public GridPosition(Vector2Int value)
        {
            Value = value;
        }

        public bool Equals(GridPosition other)
        {
            return Value.Equals(other.Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator Vector2Int(GridPosition position)
        {
            return position.Value;
        }
    }
}
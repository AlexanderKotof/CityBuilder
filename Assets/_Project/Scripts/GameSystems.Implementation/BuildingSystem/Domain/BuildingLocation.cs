using System;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;

namespace CityBuilder.GameSystems.Implementation.BuildingSystem.Domain
{
    [Obsolete]
    public readonly struct BuildingLocation : IEquatable<BuildingLocation>
    {
        public readonly GridPosition Position;

        public readonly GridModel Grid;

        public BuildingLocation(GridPosition position, GridModel grid)
        {
            Position = position;
            Grid = grid;
        }

        public bool Equals(BuildingLocation other)
        {
            return Position.Equals(other.Position) && Grid.Equals(other.Grid);
        }

        public override bool Equals(object obj)
        {
            return obj is BuildingLocation other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Position.Value.GetHashCode() * 17 + Grid.GetHashCode();
        }

        public override string ToString()
        {
            return $"Location: {Grid} : {Position.ToString()}";
        }
    }
}
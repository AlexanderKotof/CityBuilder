using System;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.CellGridFeature.Grid
{
    public interface IGridComponent : IEquatable<IGridComponent>
    {
        int GetInstanceID();

        Vector2Int Size { get; }

        Transform Transform { get; }
    }
}
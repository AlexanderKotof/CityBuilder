using System;
using UnityEngine;

namespace CityBuilder.Grid
{
    public interface IGridComponent : IEquatable<IGridComponent>
    {
        int GetInstanceID();

        Vector2Int Size { get; }

        Transform Transform { get; }
    }
}
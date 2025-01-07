using System;
using System.Collections.Generic;
using UnityEngine;

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

public interface IGridComponent : IEquatable<IGridComponent>
{
    int GetInstanceID();

    Vector2Int Size { get; }

    Transform Transform { get; }
}

public class GridComponent : MonoBehaviour, IGridComponent
{
    public Vector2Int GridSize;
    public Transform Transform => transform;
    
    public Vector2Int Size => GridSize;
    
    private bool Equals(GridComponent other)
    {
        return this.GetInstanceID() == other.GetInstanceID();
    }

    public bool Equals(IGridComponent other)
    {
        return other != null && this.GetInstanceID() == other.GetInstanceID();
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((GridComponent)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), this.GetInstanceID());
    }

    public override string ToString()
    {
        return name;
    }
}
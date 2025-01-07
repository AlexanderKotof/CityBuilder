using JetBrains.Annotations;
using SityBuilder.Reactive;
using UnityEngine;

public class CellModel
{
    public ReactiveProperty<ICellContent> Content { get; } = new ReactiveProperty<ICellContent>();
    public GridPosition Position { get; }
    public GridModel GridModel { get; }
    
    public Vector3 WorldPosition => GridModel.GridPositionToCellWorldPosition(Position);

    public CellModel(GridPosition gridPosition, GridModel gridModel)
    {
        Position = gridPosition;
        GridModel = gridModel;
    }

    public override string ToString()
    {
        return $"Cell: {Position}";
    }

    public void SetContent([CanBeNull] ICellContent building)
    {
        Content.Set(building);
    }
}
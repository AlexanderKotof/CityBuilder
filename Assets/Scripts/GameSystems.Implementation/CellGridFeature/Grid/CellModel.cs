using System;
using CityBuilder.Content;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace CityBuilder.Grid
{
    public class CellModel : IEquatable<CellModel>
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

        public override bool Equals(object obj)
        {
            return obj is CellModel cellModel && Equals(cellModel);
        }

        public override int GetHashCode()
        {
            return Position.X * 112 + Position.Y * GridModel.GetHashCode();
        }

        public bool Equals(CellModel other)
        {
            return other != null && other.GridModel == GridModel &&
                   other.Position.Equals(Position);
        }

        public override string ToString()
        {
            return $"Cell: {Position}";
        }

        public void SetContent([CanBeNull] ICellContent building)
        {
            Content.Value = (building);
        }
    }
}
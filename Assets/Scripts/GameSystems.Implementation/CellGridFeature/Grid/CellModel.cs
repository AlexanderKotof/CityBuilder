using System;
using System.Collections.Generic;
using CityBuilder.Content;
using Configs.Implementation.Common;
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

    public static class CellModelExtensions
    {
        public static IEnumerable<CellModel> Expand(this CellModel cell, Size size)
        {
            var list = new List<CellModel>();
            
            var gridModel = cell.GridModel;
            var position = cell.Position;
            
            for (int i = position.X; i < position.X + size.X; i++)
            {
                for (int j = position.Y; j < position.Y + size.Y; j++)
                {
                    list.Add(gridModel.GetCell(i, j));
                }
            }

            return list;
        }
    }
}
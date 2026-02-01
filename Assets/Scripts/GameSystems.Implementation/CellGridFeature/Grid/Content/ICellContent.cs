using System.Collections.Generic;
using CityBuilder.Grid;
using GameSystems.Implementation.BattleSystem;
using GameSystems.Implementation.BuildingSystem.Domain;
using UnityEngine;

namespace CityBuilder.Content
{
    public interface ICellContent : ICellOccupier
    {
        bool CanBeMoved { get; }
    
        bool IsEmpty { get; }
    }

    public class GridContentManager
    {
        private readonly Dictionary<CellModel, ICellContent> ContentMap = new();

        public void SetContent(CellModel cellModel, ICellContent cellContent)
        {
            ContentMap[cellModel] = cellContent;
        }
        
        public void SetContent(IEnumerable<CellModel> cellModels, ICellContent cellContent)
        {
            cellModels.ForEach(model => ContentMap[model] = cellContent);
        }
        
        public bool TryGetCellContent(CellModel cell, out ICellContent cellContent)
        {
            return ContentMap.TryGetValue(cell, out cellContent);
        }
        
        public bool TryGetCellContent<TContent>(CellModel cell, out TContent cellContent) where TContent : ICellContent
        {
            cellContent = default(TContent);
            if (TryGetCellContent(cell, out ICellContent content))
            {
                cellContent = (TContent)content;
            }
            return cellContent != null;
        }
    }
}
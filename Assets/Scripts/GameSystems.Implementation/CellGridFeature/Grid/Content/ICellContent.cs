using System.Collections.Generic;
using BuildingSystem;
using CityBuilder.Grid;
using UnityEngine;

namespace CityBuilder.Content
{
    public interface ICellContent : ICellOccupier
    {
        bool CanBeMoved { get; }
    
        bool IsEmpty { get; }
    }

    public class ContentManager
    {
        public readonly Dictionary<CellModel, ICellContent> ContentMap = new();

        public void PlaceContent(CellModel cellModel, ICellContent cellContent)
        {
            
        }
        
        public bool TryGetCellContent(CellModel cell, out ICellContent cellContent)
        {
            return ContentMap.TryGetValue(cell, out cellContent);
        }
    }
}
using System;
using System.Collections.Generic;
using CityBuilder.Content;
using CityBuilder.Grid;
using InteractionStateMachine;
using CityBuilder.Reactive;
using Unity.VisualScripting;
using UnityEngine;
using ViewSystem;

namespace CityBuilder.BuildingSystem
{
    public interface ICellOccupier
    {
        IReadOnlyCollection<CellModel> OccupiedCells { get; }
    }
    
    public class Building : ICellContent, ICellOccupier, IViewModel, IDraggableViewModel
    {
        public string BuildingName => Config.Name;
        public ReactiveProperty<int> Level { get; } = new();
        public ReactiveProperty<int> Rotation { get; } = new();
        public ReactiveProperty<Vector3> WorldPosition { get; } = new();
        
        public BuildingConfig Config { get; }
        public GameObject View { get; private set; }
        // 0-4 
        public readonly Guid Id = Guid.NewGuid();
        
        public IReadOnlyCollection<CellModel> OccupiedCells { get; private set; }
        
        public bool CanBeMoved => true;
        public bool IsEmpty => false;
        
        public Building(int level, BuildingConfig config)
        {
            Level.Set(level);
            Config = config;
        }

        public void SetOccupiedCells(IReadOnlyCollection<CellModel> occupiedCells)
        {
            OccupiedCells = occupiedCells;
        }

        public void IncreaseLevel()
        {
            Level.Set(Level.Value + 1);
        }
    }
}

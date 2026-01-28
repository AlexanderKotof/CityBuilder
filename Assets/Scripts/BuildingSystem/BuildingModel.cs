using System;
using System.Collections.Generic;
using CityBuilder.Content;
using CityBuilder.Grid;
using CityBuilder.Reactive;
using Configs.Implementation.Buildings;
using Configs.Schemes;
using Configs.Scriptable;
using GameSystems.Implementation.GameInteractionFeature;
using Unity.VisualScripting;
using UnityEngine;
using ViewSystem;

namespace CityBuilder.BuildingSystem
{
    public interface ICellOccupier
    {
        IReadOnlyCollection<CellModel> OccupiedCells { get; }
    }
    
    public class BuildingModel : ICellContent, ICellOccupier, IViewModel, IDraggableViewModel
    {
        public string BuildingName => Config.Name;
        public ReactiveProperty<int> Level { get; } = new();
        public ReactiveProperty<int> Rotation { get; } = new();
        public ReactiveProperty<Vector3> WorldPosition { get; } = new();
        
        public BuildingConfigSO Config { get; }

        //Трансформ вьюхи в игре
        public ReactiveProperty<Transform?> ThisTransform { get; } = new();

        // 0-4 
        public readonly Guid RuntimeId = Guid.NewGuid();
        
        public IReadOnlyCollection<CellModel> OccupiedCells { get; private set; }

        public bool CanBeMoved => Config.IsMovable;
        public bool IsEmpty => false;
        
        public BuildingModel(int level, BuildingConfigSO config)
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

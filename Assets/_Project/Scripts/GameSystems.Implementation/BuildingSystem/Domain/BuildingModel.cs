using System;
using System.Collections.Generic;
using CityBuilder.Configs.Scriptable.Buildings;
using CityBuilder.GameSystems.Common.ViewSystem;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.GameSystems.Implementation.GameInteractionFeature;
using UniRx;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.BuildingSystem.Domain
{
    public interface ICellOccupier
    {
        IReadOnlyCollection<CellModel> OccupiedCells { get; }
    }
    
    public class BuildingModel : ICellOccupier, IViewModel, IDraggableViewModel, IDisposable
    {
        public string BuildingName => Config.Name;
        public ReactiveProperty<int> Level { get; } = new();
        public ReactiveProperty<int> Rotation { get; } = new();
        public ReactiveProperty<Vector3> WorldPosition { get; } = new();
        public ReactiveProperty<bool> IsDragging { get; } = new();

        public bool IsMaxLevel => Level.Value >= Config.BuildingLevelingConfig.MaxLevel;
        
        public BuildingConfigSo Config { get; }

        //Трансформ вьюхи в игре
        public ReactiveProperty<Transform?> ThisTransform { get; } = new();

        // 0-4 
        public readonly Guid RuntimeId = Guid.NewGuid();
        
        public IReadOnlyCollection<CellModel> OccupiedCells { get; private set; }

        public bool CanBeMoved => Config.IsMovable;
        public bool IsEmpty => false;
        
        public BuildingModel(int level, BuildingConfigSo config)
        {
            Level.Value = (level);
            Config = config;
        }

        public void SetOccupiedCells(IReadOnlyCollection<CellModel> occupiedCells)
        {
            OccupiedCells = occupiedCells;
        }

        public void IncreaseLevel()
        {
            Level.Value = (Level.Value + 1);
        }

        public void Dispose()
        {
            Level.Dispose();
            WorldPosition.Dispose();
            ThisTransform.Dispose();
            OccupiedCells = null;
        }
    }
}

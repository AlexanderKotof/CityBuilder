using System;
using System.Collections.Generic;
using CityBuilder.Content;
using CityBuilder.Grid;
using Configs.Scriptable.Buildings;
using GameSystems.Common.ViewSystem;
using GameSystems.Implementation.GameInteractionFeature;
using UniRx;
using UnityEngine;

namespace GameSystems.Implementation.BuildingSystem.Domain
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
    }
}

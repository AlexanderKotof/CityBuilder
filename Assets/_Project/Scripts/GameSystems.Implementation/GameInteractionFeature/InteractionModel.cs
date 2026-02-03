using System.Collections.Generic;
using CityBuilder.Configs.Scriptable.Buildings.Merge;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.Utilities.Extensions;
using UniRx;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature
{
    public class InteractionModel
    {
        public IReadOnlyReactiveProperty<CellModel> HoveredCell => _hoveredCell;
        private readonly ReactiveProperty<CellModel> _hoveredCell = new(null);

        public readonly ReactiveProperty<CellModel?> SelectedCell = new(null);
        public readonly ReactiveProperty<CellModel?> DraggedCell = new(null);
        
        public readonly ReactiveCommand CancelDrag = new();
        public readonly ReactiveCommand<(CellModel from, CellModel to)> DragAndDropped = new();
        
        public readonly ReactiveProperty<IPlayerAction> SelectedAction = new();
        public readonly ReactiveCommand<IPlayerAction> ExecuteAction = new();
        
        public CellModel LastHoveredCell { get; private set; }

        public void SetHovered(CellModel cell)
        {
            _hoveredCell.Set(cell);
            LastHoveredCell = cell;
        }

        public void ClearHover()
        {
            _hoveredCell.Set(null);
        }

        public void SelectAction(IPlayerAction action)
        {
            SelectedAction.Value = action;
        }
        
        public void ExecuteCurrent()
        {
            if (SelectedAction.Value == null)
            {
                Debug.LogError("Cannot execute null action");
                return;
            }
            
            ExecuteAction.Execute(SelectedAction.Value);
        }
    }

    public interface IPlayerAction
    {
        
    }

    public record RejectedAction(CellModel ToCell): IPlayerAction
    {
        public CellModel ToCell { get; } = ToCell;
    }

    public record MoveBuildingAction(BuildingModel Building, CellModel ToCell) : IPlayerAction
    {
        public BuildingModel Building { get; } = Building;
        public CellModel ToCell { get; } = ToCell;
    }
    
    public record MergeLevelUoBuildingsAction(BuildingModel DraggedBuilding, BuildingModel ToBuilding, IReadOnlyCollection<BuildingModel> InvolvedBuildings) : IPlayerAction
    {
        public BuildingModel DraggedBuilding { get; } = DraggedBuilding;
        public BuildingModel ToBuilding { get; } = ToBuilding;
        public IReadOnlyCollection<BuildingModel> InvolvedBuildings { get; } = InvolvedBuildings;
    }
    
    public record MergeWithRecipeBuildingsAction(BuildingModel DraggedBuilding, BuildingModel ToBuilding, MergeBuildingsRecipeSo Recipe, IReadOnlyCollection<BuildingModel> InvolvedBuildings) : IPlayerAction
    {
        public BuildingModel DraggedBuilding { get; } = DraggedBuilding;
        public BuildingModel ToBuilding { get; } = ToBuilding;
        public IReadOnlyCollection<BuildingModel> InvolvedBuildings { get; } = InvolvedBuildings;
        public MergeBuildingsRecipeSo Recipe { get; } = Recipe;
    }
}
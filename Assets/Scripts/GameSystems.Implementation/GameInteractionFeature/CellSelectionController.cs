using System;
using System.Linq;
using CityBuilder.Grid;
using GameSystems.Implementation.BuildingSystem;
using GameSystems.Implementation.BuildingSystem.Features;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using JetBrains.Annotations;
using UniRx;
using VContainer.Unity;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class CellSelectionController : IInitializable, IDisposable
    {
        private readonly CursorController _cursorController;
        private readonly InteractionModel _interactionModel;
        private readonly BuildingManager _buildingManager;
        private readonly MergeBuildingsFeature _mergeBuildingsFeature;
        private readonly CompositeDisposable _disposables = new();

        public CellSelectionController(CursorController cursorController, InteractionModel interactionModel, BuildingManager buildingManager, MergeBuildingsFeature mergeBuildingsFeature)
        {
            _cursorController = cursorController;
            _interactionModel = interactionModel;
            _buildingManager = buildingManager;
            _mergeBuildingsFeature = mergeBuildingsFeature;
        }

        public void Initialize()
        {
            _interactionModel.SelectedCell.Subscribe(OnCellSelected).AddTo(_disposables);
            _interactionModel.DraggedCell.Subscribe(OnDraggingCell).AddTo(_disposables);
            _interactionModel.HoveredCell.Subscribe(OnHoveredCellChanged).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
        
        private void OnHoveredCellChanged([CanBeNull] CellModel cell)
        {
            if (cell == null || _interactionModel.DraggedCell.Value == null)
            {
                return;
            }
            
            if (!_buildingManager.TryGetBuilding(_interactionModel.DraggedCell.Value, out var fromBuilding))
                return;

            if (_buildingManager.CanPlaceBuilding(cell, fromBuilding))
            {
                _cursorController.Clear();
                _cursorController.SetPositions(cell.Expand(fromBuilding.Config.Size), CursorStateEnum.Accepted);
                return;
            }
            
            if (!_buildingManager.TryGetBuilding(cell, out var toBuilding))
                return;

            if (_mergeBuildingsFeature.CanLevelUpMerge(toBuilding, fromBuilding, out var buildings))
            {
                _cursorController.Clear();
                _cursorController.SetPositions(buildings.Append(fromBuilding).Append(toBuilding).SelectMany(building => building.OccupiedCells).Distinct(), CursorStateEnum.Upgrade);
                return;
            }
            
            if (_mergeBuildingsFeature.CanRecipeMerge(toBuilding, fromBuilding, out _, out buildings))
            {
                _cursorController.Clear();
                _cursorController.SetPositions(buildings.Append(fromBuilding).Append(toBuilding).SelectMany(building => building.OccupiedCells).Distinct(), CursorStateEnum.Merge);
                return;
            }
            
            _cursorController.Clear();
            _cursorController.SetPosition(cell, CursorStateEnum.Rejected);
        }
        
        private void OnCellSelected(CellModel cellModel)
        {
            _cursorController.Clear();

            if (cellModel == null)
                return;
            
            if (_buildingManager.TryGetBuilding(cellModel, out var building))
            {
                _cursorController.SetPositions(building.OccupiedCells, CursorStateEnum.Selection); 
            }
            else
            {
                _cursorController.SetPosition(cellModel, CursorStateEnum.Selection); 
            }
        }
        
        private void OnDraggingCell(CellModel cellModel)
        {
            //TODO: something 
            _cursorController.Clear();

            if (cellModel == null)
            {
                OnCellSelected(_interactionModel.SelectedCell.Value);
            }
        }
    }
}
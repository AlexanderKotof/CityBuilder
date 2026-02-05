using System;
using System.Linq;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature
{
    public class CellSelectionController : IInitializable, IDisposable
    {
        private readonly CursorController _cursorController;
        private readonly InteractionModel _interactionModel;
        private readonly CompositeDisposable _disposables = new();

        public CellSelectionController(CursorController cursorController, InteractionModel interactionModel)
        {
            _cursorController = cursorController;
            _interactionModel = interactionModel;
        }

        public void Initialize()
        {
            _interactionModel.SelectedCell.Subscribe(OnCellSelected).AddTo(_disposables);
            _interactionModel.DraggedCell.Subscribe(OnDraggingCell).AddTo(_disposables);
            _interactionModel.SelectedAction.Skip(1).Subscribe(OnSelectedActionChanged).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
        
        private void OnSelectedActionChanged(IPlayerAction action)
        {
            switch (action)
            {
                case MoveBuildingAction moveBuildingAction:
                    _cursorController.Clear();
                    _cursorController.SetPositions(
                        moveBuildingAction.ToCell.Expand(moveBuildingAction.Building.Config.Size), 
                        CursorStateEnum.Accepted);
                    break;
                case MergeLevelUoBuildingsAction mergeLevelUoBuildingsAction:
                    _cursorController.Clear();
                    var buildings = mergeLevelUoBuildingsAction.InvolvedBuildings;
                    _cursorController.SetPositions(
                        buildings.SelectMany(building => building.OccupiedCells).Distinct(),
                        CursorStateEnum.Upgrade);
                    break;
                case MergeWithRecipeBuildingsAction mergeWithRecipeBuildingsAction:
                    _cursorController.Clear();
                    buildings = mergeWithRecipeBuildingsAction.InvolvedBuildings;
                    _cursorController.SetPositions(buildings.SelectMany(building => building.OccupiedCells).Distinct(), CursorStateEnum.Merge);
                    break;
                
                case RejectedAction rejectedAction:
                    _cursorController.Clear();
                    _cursorController.SetPosition(rejectedAction.ToCell, CursorStateEnum.Rejected);
                    break;
                
                default:
                   Debug.LogError("Not supported action");
                    break;
            }
        }
        
        private void OnCellSelected(CellModel cellModel)
        {
            _cursorController.Clear();

            if (cellModel == null)
                return;

            var content = cellModel.Content.Value;
            if (content is ICellOccupier occupier)
            {
                _cursorController.SetPositions(occupier.OccupiedCells, CursorStateEnum.Selection); 
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
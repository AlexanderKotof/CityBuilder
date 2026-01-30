using System;
using CityBuilder.Grid;
using Configs.Implementation.Common;
using GameSystems.Implementation.BuildingSystem;
using VContainer.Unity;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class CellSelectionController : IInitializable, IDisposable
    {
        private readonly CursorController _cursorController;
        private readonly InteractionModel _interactionModel;
        private readonly BuildingManager _buildingManager;
        
        public CellSelectionController(CursorController cursorController, InteractionModel interactionModel, BuildingManager buildingManager)
        {
            _cursorController = cursorController;
            _interactionModel = interactionModel;
            _buildingManager = buildingManager;
        }

        public void Initialize()
        {
            _interactionModel.SelectedCell.Subscribe(OnCellSelected);
            _interactionModel.DraggedCell.Subscribe(OnDraggingCell);
        }

        public void Dispose()
        {
            _interactionModel.SelectedCell.Unsubscribe(OnCellSelected);
            _interactionModel.DraggedCell.Unsubscribe(OnDraggingCell);
        }
        
        private void OnCellSelected(CellModel cellModel)
        {
            if (cellModel == null)
                return;
            
            if (_buildingManager.TryGetBuilding(cellModel, out var building))
            {
                _cursorController.SetActive(true);
                _cursorController.SetPosition(building.WorldPosition.Value, building.Config.Size.ToVector()); 
            }
            else
            {
                _cursorController.SetActive(false);
            }
        }
        
        private void OnDraggingCell(CellModel cellModel)
        {
            //TODO: something 
        }
    }
}
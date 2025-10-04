using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using UnityEngine;

namespace InteractionStateMachine
{
    public class DraggingInteractionState : InteractionState
    {
        private readonly DraggingContentController _draggingContentController;
        private readonly BuildingManager _buildingManager;
        private readonly GridManager _gridManager;

        public DraggingInteractionState(IDependencyContainer dependencyContainer) : base(dependencyContainer)
        {
            _draggingContentController = dependencyContainer.Resolve<DraggingContentController>();
            _buildingManager = dependencyContainer.Resolve<BuildingManager>();
        }

        protected override void OnEnterState()
        {
            base.OnEnterState();
            
            //ToDo content manager
            if (_buildingManager.TryGetBuilding(InteractionModel.DraggedCell.Value, out var building))
            {
                _draggingContentController.StartDraggingContent(building);
            }
            else
            {
                ChangeState<EmptyInteractionState>();
            }
        }

        protected override void OnExitState()
        {
            base.OnExitState();
            _draggingContentController.EndDragging();
            InteractionModel.DraggedCell.Set(null);
        }

        protected override void ProcessDragging(Vector3 mousePosition)
        {
            if (Raycaster.TryGetCursorPositionFromScreenPoint(mousePosition, out var cursorPosition))
            {
                _draggingContentController.UpdatePosition(cursorPosition.Value);
            }
        }
        
        protected override void ProcessDragEnded(CellModel cellModel)
        {
            base.ProcessDragEnded(cellModel);
            
            if (TryDropContent(cellModel))
            {
                SelectCell(cellModel);
                ChangeState<CellSelectedInteractionState>();
            }
            else
            {
                _draggingContentController.CancelDrag();
                ChangeState<EmptyInteractionState>();
            }
        }

        private bool TryDropContent(CellModel cellModel)
        {
            if (InteractionModel.DraggedCell == null ||
                cellModel == null ||
                Equals(InteractionModel.DraggedCell.Value, cellModel))
            {
                return false;
            }
            
            return _buildingManager.TryDragCellFromTo(InteractionModel.DraggedCell.Value, cellModel);;
        }
    }
}
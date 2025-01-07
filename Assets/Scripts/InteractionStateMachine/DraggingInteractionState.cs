using BuildingSystem;
using UnityEngine;

namespace InteractionStateMachine
{
    public class DraggingInteractionState : InteractionState
    {
        private readonly DraggingContentController _draggingContentController;
        private readonly BuildingManager _buildingManager;
        private readonly GridManager _gridManager;

        public DraggingInteractionState(Dependencies dependencies) : base(dependencies)
        {
            _draggingContentController = dependencies.Resolve<DraggingContentController>();
            _buildingManager = dependencies.Resolve<BuildingManager>();
        }

        protected override void OnEnterState()
        {
            base.OnEnterState();
            _draggingContentController.StartDraggingContent(InteractionModel.DraggedCell);
        }

        protected override void OnExitState()
        {
            base.OnExitState();
            _draggingContentController.EndDragging();
            InteractionModel.DraggedCell = null;
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
                _draggingContentController.CancelDragging();
                ChangeState<EmptyInteractionState>();
            }
        }

        private bool TryDropContent(CellModel cellModel)
        {
            if (InteractionModel.DraggedCell == null ||
                cellModel == null ||
                InteractionModel.DraggedCell == cellModel)
            {
                return false;
            }
            
            return _buildingManager.TryDragCellFromTo(InteractionModel.DraggedCell, cellModel);;
        }
    }
}
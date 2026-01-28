using CityBuilder.Grid;
using UnityEngine;
using VContainer;

namespace GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States
{
    public class DraggingInteractionState : InteractionState
    {
        [Inject]
        private readonly DraggingContentController _draggingContentController;
        [Inject]
        private readonly GridManager _gridManager;

        protected override void OnEnterState()
        {
            base.OnEnterState();
            
            //ToDo content manager
            if (BuildingManager.TryGetBuilding(InteractionModel.DraggedCell.Value, out var building))
            {
                _draggingContentController.StartDraggingContent(building);
            }
            else
            {
                ChangeState<EmptyInteractionState>();
            }
        }

        public override void Update()
        {
            LightenCellUnderCursor();
        }

        protected override void OnExitState()
        {
            base.OnExitState();
            _draggingContentController.EndDragging();
            InteractionModel.DraggedCell.Set(null);
            InteractionModel.SelectedCell.Notify();
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
            
            return BuildingManager.TryDragCellFromTo(InteractionModel.DraggedCell.Value, cellModel);;
        }
    }
}
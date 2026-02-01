using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.Utilities.Extensions;
using UnityEngine;
using VContainer;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States
{
    public class DraggingInteractionState : InteractionState
    {
        [Inject]
        private readonly DraggingContentController _draggingContentController;

        [Inject]
        private readonly GameInteractionFeature _gameInteractionFeature;

        protected override void OnEnterState()
        {
            base.OnEnterState();

            var content = InteractionModel.DraggedCell.Value?.Content?.Value;
            if (content is IDraggableViewModel draggableViewModel)
            {
                _draggingContentController.StartDraggingContent(draggableViewModel);
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
            if (Raycaster.TryGetFreePositionFromScreenPoint(mousePosition, out var cursorPosition))
            {
                _draggingContentController.UpdatePosition(cursorPosition.Value);
            }
        }
        
        protected override void ProcessDragEnded(CellModel cellModel)
        {
            base.ProcessDragEnded(cellModel);
            
            if (TryDropContent(InteractionModel.DraggedCell?.Value, cellModel))
            {
                SelectCell(cellModel);
            }
            else
            {
                _draggingContentController.CancelDrag();
            }
            
            ChangeState<CellSelectedInteractionState>();
        }

        private bool TryDropContent(CellModel fromCell, CellModel toCellModel)
        {
            if (fromCell == null ||
                toCellModel == null ||
                Equals(fromCell, toCellModel))
            {
                return false;
            }
            
            return _gameInteractionFeature.TryDropContent(fromCell, toCellModel);
        }
    }
}
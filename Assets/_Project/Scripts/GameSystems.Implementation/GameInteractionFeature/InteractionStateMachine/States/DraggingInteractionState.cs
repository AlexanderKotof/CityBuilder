using System;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.Utilities.Extensions;
using UniRx;
using UnityEngine;
using VContainer;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States
{
    public class DraggingInteractionState : InteractionState
    {
        [Inject]
        private readonly DraggingContentController _draggingContentController;
        
        private IDisposable _subscription;

        protected override void OnEnterState()
        {
            base.OnEnterState();

            var content = InteractionModel.DraggedCell.Value?.Content?.Value;
            if (content is IDraggableViewModel draggableViewModel)
            {
                _draggingContentController.StartDraggingContent(draggableViewModel);
                _subscription?.Dispose();
                _subscription = InteractionModel.CancelDrag.Subscribe(_ => CancelDrag());
            }
            else
            {
                ChangeState<EmptyInteractionState>();
            }
        }

        protected override void OnExitState()
        {
            base.OnExitState();
            _subscription?.Dispose();
            _subscription = null;
            
            _draggingContentController.EndDragging();
            InteractionModel.DraggedCell.Set(null);
        }

        protected override void ProcessDragging(Vector2 mousePosition)
        {
            if (Raycaster.TryGetFreePositionFromScreenPoint(mousePosition, out var cursorPosition))
            {
                _draggingContentController.UpdatePosition(cursorPosition.Value);
            }
        }
        
        protected override void ProcessDragEnded(CellModel cellModel)
        {
            base.ProcessDragEnded(cellModel);

            TryDropContent(InteractionModel.DraggedCell?.Value, cellModel);
           
            ChangeState<EmptyInteractionState>();
        }

        private void CancelDrag()
        {
            _draggingContentController.CancelDrag();
            ChangeState<EmptyInteractionState>();
        }

        private void TryDropContent(CellModel fromCell, CellModel toCellModel)
        {
            if (fromCell == null ||
                toCellModel == null ||
                Equals(fromCell, toCellModel))
            {
                CancelDrag();
                return;
            }
            
            InteractionModel.DragAndDropped.Execute((fromCell, toCellModel));
        }
    }
}
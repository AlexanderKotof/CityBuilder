using System;
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
        private readonly GameInteractionFeature _gameInteractionFeature;

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
            //TODO: pass content size
            LightenCellUnderCursor(Vector2Int.one);
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
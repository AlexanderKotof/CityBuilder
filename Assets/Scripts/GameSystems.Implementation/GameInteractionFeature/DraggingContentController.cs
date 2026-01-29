using GameSystems.Common.ViewSystem;
using UniRx;
using UnityEngine;
using ViewSystem;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public interface IDraggableViewModel : IViewModel
    {
        ReactiveProperty<Vector3> WorldPosition { get; }
    }
    
    public class DraggingContentController
    {
        private readonly Vector3 _draggingOffset = new Vector3(0, 1.2f, 0);
        
        private IDraggableViewModel _draggable;
        
        private Vector3 _startDragPosition;

        public void StartDraggingContent(IDraggableViewModel draggableViewModel)
        {
            _draggable = draggableViewModel;
            _startDragPosition = draggableViewModel.WorldPosition.Value;
        }

        public void EndDragging()
        {
            _draggable = null;
        }

        public void UpdatePosition(Vector3 gridPosition)
        {
            _draggable.WorldPosition.Value = (gridPosition + _draggingOffset);
        }

        public void CancelDrag()
        {
            if (_draggable == null)
            {
                return;
            }
            _draggable.WorldPosition.Value = (_startDragPosition);
            _draggable = null;
        }
    }
}
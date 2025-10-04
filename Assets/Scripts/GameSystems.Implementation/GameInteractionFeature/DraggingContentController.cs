using CityBuilder.Reactive;
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
            _startDragPosition = draggableViewModel.WorldPosition;
        }

        public void EndDragging()
        {
            _draggable = null;
        }

        public void UpdatePosition(Vector3 gridPosition)
        {
            _draggable.WorldPosition.Set(gridPosition + _draggingOffset);
        }

        public void CancelDrag()
        {
            if (_draggable == null)
            {
                return;
            }
            _draggable.WorldPosition.Set(_startDragPosition);
            _draggable = null;
        }
    }
}
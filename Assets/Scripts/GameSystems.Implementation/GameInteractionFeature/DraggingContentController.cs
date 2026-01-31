using GameSystems.Common.ViewSystem;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using ViewSystem;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public interface IDraggableViewModel : IViewModel
    {
        ReactiveProperty<Vector3> WorldPosition { get; }
        
        ReactiveProperty<bool> IsDragging { get; }
    }

    public class DraggingContentController : ITickable
    {
        private readonly Vector3 _draggingOffset = new Vector3(0, 1.2f, 0);

        private IDraggableViewModel _draggable;

        private Vector3 _startDragPosition;
        private Vector3 _targetPosition;

        public void StartDraggingContent(IDraggableViewModel draggableViewModel)
        {
            _draggable = draggableViewModel;
            _draggable.IsDragging.Value = true;
            _startDragPosition = _targetPosition = draggableViewModel.WorldPosition.Value;
        }

        public void EndDragging()
        {
            if (_draggable != null)
            {
                _draggable.IsDragging.Value = false;
                _draggable = null;
            }
        }
        
        public void Tick()
        {
            if (_draggable == null)
            {
                return;
            }
            
            _draggable.WorldPosition.Value = Vector3.Lerp(_draggable.WorldPosition.Value, _targetPosition, 0.5f);
        }
        
        public void UpdatePosition(Vector3 gridPosition)
        {
            _targetPosition = gridPosition + _draggingOffset;
        }

        public void CancelDrag()
        {
            if (_draggable == null)
            {
                return;
            }
            
            _draggable.WorldPosition.Value = _targetPosition = _startDragPosition;
            EndDragging();
        }
    }
}
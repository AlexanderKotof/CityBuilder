using UnityEngine;
using ViewSystem;

namespace InteractionStateMachine
{
    public class DraggingContentController
    {
        private readonly Transform _rootTransform;
        private readonly ViewsProvider _viewsProvider;

        private readonly Vector3 _draggingOffset = new Vector3(0, 3, 0);

        private GameObject _spawnedView;
        private Transform _draggingTransform;
        private GameObject _hidenView;

        public DraggingContentController(Transform rootTransform, ViewsProvider viewsProvider)
        {
            _rootTransform = rootTransform;
            _viewsProvider = viewsProvider;
        }

        public void StartDraggingContent(CellModel cell)
        {
            _spawnedView = _viewsProvider.ProvideView(cell.Content.Value.View, _rootTransform);
            _spawnedView.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            _hidenView = cell.Content.Value.View;
            _hidenView.SetActive(false);
        }

        public void EndDragging()
        {
            if (_spawnedView == null)
            {
                return;
            }
            _viewsProvider.ReturnView(_spawnedView);
        }

        public void CancelDragging()
        {
            _hidenView.SetActive(true);
        }

        public void UpdatePosition(Vector3 gridPosition)
        {
            _rootTransform.position = gridPosition + _draggingOffset;
        }
    }
}
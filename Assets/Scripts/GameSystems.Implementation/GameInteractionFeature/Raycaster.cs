using System.Diagnostics.CodeAnalysis;
using CityBuilder.Grid;
using Configs.Scriptable;
using UnityEngine;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class Raycaster
    {
        private readonly Camera _raycastCamera;
        private readonly LayerMask _layerMask;
        private readonly GridManager _gridManager;

        public Raycaster(Camera raycastCamera, InteractionSettingsSo settings, GridManager gridManager)
        {
            _raycastCamera = raycastCamera;
            _layerMask = settings.InteractionRaycastLayerMask;
            _gridManager = gridManager;
        }

        public bool TryGetCellFromScreenPoint(Vector2 screenPoint, [NotNullWhen(true)] out CellModel? cell)
        {
            var ray = _raycastCamera.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out var hit, _layerMask))
            {
                var grid = hit.collider.GetComponentInParent<GridComponent>();
                if (grid && TryGetCellFromHitPoint(hit.point, grid, out cell))
                {
                    return true;
                }
            }
        
            cell = null;
            return false;
        }
    
        public bool TryGetCursorPositionFromScreenPoint(Vector2 screenPoint, [NotNullWhen(true)] out Vector3? cursorPosition)
        {
            var ray = _raycastCamera.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out var hit, _layerMask))
            {
                var component = hit.collider.GetComponentInParent<GridComponent>();
                if (component != null)
                {
                    cursorPosition = GetCursorPositionFromHitPoint(hit.point, component);
                    return true;
                }
            }
            cursorPosition = null;
            return false;
        }
        
        public bool TryGetFreePositionFromScreenPoint(Vector2 screenPoint, [NotNullWhen(true)] out Vector3? cursorPosition)
        {
            var ray = _raycastCamera.ScreenPointToRay(screenPoint);
            if (Physics.Raycast(ray, out var hit, _layerMask))
            {
                cursorPosition = hit.point;
                return true;
            }
            cursorPosition = null;
            return false;
        }
    
        private bool TryGetCellFromHitPoint(Vector3 hitPoint, IGridComponent component, [NotNullWhen(true)] out CellModel? cell)
        {
            Vector3 localHitPosition = component.Transform.InverseTransformPoint(hitPoint);
            Vector2Int hitPosition2d = new Vector2Int(Mathf.FloorToInt(localHitPosition.x),
                Mathf.FloorToInt(localHitPosition.z));
        
            if (_gridManager.TryGetGridModel(component, out var grid))
            {
                return grid.TryGetCell(hitPosition2d, out cell);
            }

            cell = null;
            return false;
        }
        private bool TryGetCellFromHitPoint(Vector3 hitPoint, GridModel gridModel, [NotNullWhen(true)] out CellModel? cell)
        {
            Vector3 localHitPosition = gridModel.Transform.InverseTransformPoint(hitPoint);
            Vector2Int hitPosition2d = new Vector2Int(Mathf.FloorToInt(localHitPosition.x),
                Mathf.FloorToInt(localHitPosition.z));

            return gridModel.TryGetCell(hitPosition2d, out cell);
        }

        private Vector3 GetCursorPositionFromHitPoint(Vector3 hitPoint, GridComponent component)
        {
            Vector3 localHitPosition = component.Transform.InverseTransformPoint(hitPoint);
            Vector3 hitPosition2d = new Vector3(Mathf.FloorToInt(localHitPosition.x),
                0,
                Mathf.FloorToInt(localHitPosition.z));

            return component.Transform.TransformPoint(hitPosition2d);
        }

  
    }
}

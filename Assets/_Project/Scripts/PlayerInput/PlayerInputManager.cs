using System;
using CityBuilder.Configs.Scriptable;
using UnityEngine;

namespace CityBuilder.PlayerInput
{
    //TODO: rework with new InputSystem
    public class PlayerInputManager
    {
        public event Action<Vector3> OnMouseClick;

        public event Action<Vector3> OnMouseDragStarted;

        public event Action<Vector3> OnMouseDragging;

        public event Action<Vector3> OnMouseDragEnded;
        
        public event Action<Vector3> OnMouseRightClick;

        public Vector3 PointerPosition => Input.mousePosition;

        private bool _isDragging = false;

        private readonly float _startDragDelay;
        private readonly float _dragThresholdSqr;
        
        private float _pressTime;
        private Vector3 _pressPosition;

        public PlayerInputManager(InteractionSettingsSo settings)
        {
            _startDragDelay = settings.StartDragDelay;
            _dragThresholdSqr = settings.StartDragThreshold * settings.StartDragThreshold;
        }

        public void Update()
        {
            UpdateLeftMouseInput();
            UpdateRightMouseInput();
        }

        private void UpdateRightMouseInput()
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnMouseRightClick?.Invoke(PointerPosition);
            }
        }

        private void UpdateLeftMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseClick?.Invoke(PointerPosition);
                _pressTime = Time.realtimeSinceStartup;
                _pressPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && (CheckDelay() || CheckPosition() || _isDragging))
            {
                UpdateDragging(true);
                OnMouseDragging?.Invoke(PointerPosition);
                return;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                UpdateDragging(false);
            }
        }

        private bool CheckDelay()
        {
            return Time.realtimeSinceStartup - _pressTime > _startDragDelay;
        }
        
        private bool CheckPosition()
        {
            return (Input.mousePosition - _pressPosition).sqrMagnitude > _dragThresholdSqr;
        }

        private void UpdateDragging(bool isDragging)
        {
            if (_isDragging != isDragging)
            {
                _isDragging = isDragging;
                if (_isDragging)
                {
                    OnMouseDragStarted?.Invoke(PointerPosition);
                }
                else
                {
                    OnMouseDragEnded?.Invoke(PointerPosition);
                }
            }
        }
    }
}

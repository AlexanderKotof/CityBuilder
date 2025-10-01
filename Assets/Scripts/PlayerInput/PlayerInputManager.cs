using System;
using UnityEngine;

namespace PlayerInput
{
    public class PlayerInputManager
    {
        public event Action<Vector3> OnMouseClick;

        public event Action<Vector3> OnMouseDragStarted;

        public event Action<Vector3> OnMouseDragging;

        public event Action<Vector3> OnMouseDragEnded;
        
        public event Action<Vector3> OnMouseRightClick;

        public Vector3 MousePosition => Input.mousePosition;

        private bool _isDragging = false;
        
        private const float _dragThreashold = .1f;
        private float _pressTime;

        public void Update()
        {
            UpdateLeftMouseInput();
            UpdateRightMouseInput();
        }

        private void UpdateRightMouseInput()
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnMouseRightClick?.Invoke(MousePosition);
            }
        }

        private void UpdateLeftMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseClick?.Invoke(MousePosition);
                _pressTime = Time.realtimeSinceStartup;
            }

            if (Input.GetMouseButton(0) && Time.realtimeSinceStartup - _pressTime > _dragThreashold)
            {
                UpdateDragging(true);
                OnMouseDragging?.Invoke(MousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                UpdateDragging(false);
            }
        }

        private void UpdateDragging(bool isDragging)
        {
            if (_isDragging != isDragging)
            {
                _isDragging = isDragging;
                if (_isDragging)
                {
                    OnMouseDragStarted?.Invoke(MousePosition);
                }
                else
                {
                    OnMouseDragEnded?.Invoke(MousePosition);
                }
            }
        }
    }
}

using System;
using CityBuilder.Configs.Scriptable;
using UniRx;
using UnityEngine;

namespace CityBuilder.PlayerInput
{
    //TODO: rework with new InputSystem
    public class PlayerInputManager
    {
        private readonly InteractionSettingsSo _settings;
        public event Action<Vector2> OnMouseClick;

        public event Action<Vector2> OnMouseDragStarted;

        public event Action<Vector2> OnMouseDragging;

        public event Action<Vector2> OnMouseDragEnded;
        
        public event Action<Vector2> OnMouseRightClick;

        public IObservable<Vector2> RightMouseDragAsObservable() => _rightMouseDragSubject;
        private readonly Subject<Vector2> _rightMouseDragSubject = new();

        public Vector2 PointerPosition => Input.mousePosition;
        public Vector2 PointerDeltaPosition => PointerPosition - _previousPointerPosition;

        private bool _isDragging = false;

        private readonly float _startDragDelay;
        private readonly float _dragThresholdSqr;
        
        private float _pressTime;
        private Vector2 _pressPosition;
        private Vector2 _previousPointerPosition;

        public PlayerInputManager(InteractionSettingsSo settings)
        {
            _settings = settings;
            _startDragDelay = settings.StartDragDelay;
            _dragThresholdSqr = settings.StartDragThreshold * settings.StartDragThreshold;
        }

        public void Update()
        {
            UpdateLeftMouseInput();
            UpdateRightMouseInput();
            
            _previousPointerPosition = PointerPosition;
        }

        private void UpdateRightMouseInput()
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnMouseRightClick?.Invoke(PointerPosition);
            }

            if (Input.GetMouseButton(1) && 
                PointerDeltaPosition.sqrMagnitude > _settings.CameraDragThreshold * _settings.CameraDragThreshold)
            {
                _rightMouseDragSubject.OnNext(PointerDeltaPosition);
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
            return ((Vector2)Input.mousePosition - _pressPosition).sqrMagnitude > _dragThresholdSqr;
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

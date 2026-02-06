using System;
using CityBuilder.Configs.Scriptable;
using CityBuilder.PlayerInput;
using CityBuilder.Utilities.Extensions;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CityBuilder.Installers
{
    public class CameraSystem : IInitializable, IDisposable, ILateTickable
    {
        private readonly Camera _gameCamera;
        private readonly Transform _cameraRoot;
        private readonly PlayerInputManager _inputManager;
        private readonly CommonGameSettingsSo _settings;
        private readonly CompositeDisposable _disposables = new();
        
        private Vector3 _targetPosition;
        private readonly float _lerpSpeed = 0.5f;

        public CameraSystem([Key("CameraRoot")] Transform root, Camera gameCamera, PlayerInputManager inputManager, CommonGameSettingsSo settings)
        {
            _gameCamera = gameCamera;
            _inputManager = inputManager;
            _settings = settings;
            _cameraRoot = root;
            _targetPosition = _cameraRoot.position;
        }
        
        public void Initialize()
        {
            _inputManager.RightMouseDragAsObservable()
                .Subscribe(OnRightMouseDrag)
                .AddTo(_disposables);
        }

        public void Dispose()
        { 
            _disposables.Dispose();
        }

        private void OnRightMouseDrag(Vector2 delta)
        {
            var newPosition = _cameraRoot.position -
                              _cameraRoot.TransformDirection(delta.ToXOY()) * _settings.CameraMovementSensitivity;
            
            newPosition.x = Mathf.Clamp(newPosition.x, _settings.MinCameraPosition.x, _settings.MaxCameraPosition.x);
            newPosition.z = Mathf.Clamp(newPosition.z, _settings.MinCameraPosition.z, _settings.MaxCameraPosition.z);

            _targetPosition = newPosition;
        }

        public void LateTick()
        {
            _cameraRoot.position = Vector3.Lerp(_cameraRoot.position, _targetPosition, _lerpSpeed);
        }
    }
}
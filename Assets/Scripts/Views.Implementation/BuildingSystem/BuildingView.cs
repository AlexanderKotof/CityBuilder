using System;
using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem.View;
using GameSystems.Implementation.BuildingSystem.Domain;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UniRx;
using UnityEngine;

namespace Views.Implementation.BuildingSystem
{
    public class BuildingView : ViewWithModel<BuildingModel>
    {
        public GameObject[] _visualsByLevel;
        
        public Canvas UICanvas;
        
        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;
       
        public MergeAnimationConfig MergeConfig;

        private BuildingModel _model;
        private bool _isDragging;
        private MotionHandle? _tween;

        private GameObject CurrentVisual => _visualsByLevel[Mathf.Min(_model.Level.Value, _visualsByLevel.Length - 1)];
        
        public void Initialize(BuildingModel model)
        {
            _model = model;
            model.Level.Subscribe(OnLevelUpdated).AddTo(this);
            model.WorldPosition.Subscribe(SetWorldPosition).AddTo(this);
            model.IsDragging.Subscribe(OnIsDraggingChanged).AddTo(this);
            
            NameText.SetText(model.BuildingName);

            SetUiActive(false);
        }

        private void OnIsDraggingChanged(bool value)
        {
            if (_isDragging == value)
                return;
            
            _isDragging = value;
            
            _tween = LMotion.Create(Vector3.one, Vector3.one, 0.3f)
                .WithEase(Ease.InBounce)
                .BindToLocalScale(CurrentVisual.transform);
        }

        private void OnLevelUpdated(int value)
        {
            LevelIndicator.SetText($"Lvl {(value + 1).ToString()}");
            
            if (_visualsByLevel == null || _visualsByLevel.Length == 0)
                return;
            
            for (var index = 0; index < _visualsByLevel.Length; index++)
            {
                _visualsByLevel[index].SetActive(false);
            }
            
            CurrentVisual.SetActive(true);
        }

        private void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetUiActive(bool value)
        {
            UICanvas.enabled = value;
        }

        public async UniTask MergeTo(Vector3 toPosition)
        {
            SetUiActive(false);
            
            var visualTransform = CurrentVisual.transform;
            
            var localPosition = visualTransform.localPosition;
            var startPosition = visualTransform.position;
            var startScale = visualTransform.localScale;

            await UniTask.WhenAll(
                LMotion.Create(startScale, Vector3.one * 0.3f, MergeConfig.ScalingDuration)
                    .WithDelay(MergeConfig.ScalingDelay)
                    .WithEase(MergeConfig.ScalingEase)
                    .BindToLocalScale(visualTransform)
                    .ToUniTask(),
                
                LMotion.Create(startPosition, toPosition, MergeConfig.MovingDuration)
                    .WithDelay(MergeConfig.MovingDelay)
                    .WithEase(MergeConfig.MovingEase)
                    .BindToPosition(visualTransform)
                    .ToUniTask()
                );
            
            CurrentVisual.SetActive(false);

            visualTransform.localPosition = localPosition;
            visualTransform.localScale = startScale;
        }
    }
}
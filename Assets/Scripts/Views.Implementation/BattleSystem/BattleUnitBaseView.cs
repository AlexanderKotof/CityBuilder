using System;
using CityBuilder.Dependencies;
using GameSystems.Implementation.BattleSystem;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BattleSystem
{
    public class BattleUnitBaseView : ViewWithModel<BattleUnitModel>
    {
        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;

        public override void Initialize(BattleUnitModel model, IDependencyContainer container)
        {
            base.Initialize(model, container);
            
           
            // Subscribe(model.Level, (value) => LevelIndicator.SetText($"Lvl {value}"));
            // Subscribe(model.WorldPosition, SetWorldPosition);
            // NameText.SetText(model.BuildingName);
        }

        private void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }
    }

    public class BattleUnitNavigationComponent : ViewWithModel<BattleUnitModel>
    {
        public Transform ThisTransform;
        
        private Transform? _targetTransform;

        public override void Initialize(BattleUnitModel model, IDependencyContainer dependencies)
        {
            base.Initialize(model, dependencies);
            
            model.ThisTransform.Set(ThisTransform);
            
            Subscribe(model.Target, OnTargetUpdated);
        }

        private void OnTargetUpdated(BattleUnitModel target)
        {
            _targetTransform = target.ThisTransform.Value;
        }

        private void Update()
        {
            if (_targetTransform == null)
            {
                return;
            }
            
            var direction = _targetTransform.position - ThisTransform.position;
            var targetPosition = ThisTransform.position + direction - direction.normalized * Model.Config.AttackRange * 0.9f;
            
            var newPosition = ThisTransform.position = 
                Vector3.Lerp(ThisTransform.position, targetPosition, Time.deltaTime * Model.Config.MoveSpeed);
            Model.CurrentPosition.Set(newPosition);
        }
    }
}
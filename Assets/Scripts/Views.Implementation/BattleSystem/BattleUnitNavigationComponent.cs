using CityBuilder.Dependencies;
using GameSystems.Implementation.BattleSystem;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BattleSystem
{
    public class BattleUnitNavigationComponent : ViewWithModel<BattleUnitBase>
    {
        private Transform? _targetTransform;

        public override void Initialize(BattleUnitBase model, IDependencyContainer dependencies)
        {
            base.Initialize(model, dependencies);

            if (model.AttackModel == null)
            {
                return;
            }
            
            Subscribe(model.AttackModel.Target, OnTargetUpdated);
        }

        private void OnTargetUpdated(IBattleUnit target)
        {
            if (target == null)
            {
                return;
            }
            
            _targetTransform = target.ThisTransform.Value;
        }

        private void Update()
        {
            if (_targetTransform == null)
            {
                return;
            }

            var tr = Model.ThisTransform.Value;
            var direction = _targetTransform.position - tr.position;
            var targetPosition = tr.position + direction - direction.normalized * Model.Config.AttackRange * 0.9f;
            
            var newPosition = tr.position = 
                Vector3.Lerp(tr.position, targetPosition, Time.deltaTime * Model.Config.MoveSpeed);
            
            //tr.position = newPosition;
            
            //Model.CurrentPosition.Set(newPosition);
        }
    }
}
using CityBuilder.Dependencies;
using GameSystems.Common.ViewSystem.View;
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

        private void ProcessMove()
        {
            if (_targetTransform == null)
            {
                return;
            }
            
            //TODO: move unit with navigation?
        }
    }
}
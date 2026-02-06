using CityBuilder.Dependencies;
using CityBuilder.GameSystems.Common.ViewSystem.View;
using CityBuilder.GameSystems.Implementation.BattleSystem;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;
using UnityEngine;
using UnityEngine.AI;

namespace CityBuilder.Views.Implementation.BattleSystem
{
    public class BattleUnitNavigationComponent : ViewWithModel<BattleUnitBase>
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;

        public override void Initialize(BattleUnitBase model, IDependencyContainer dependencies)
        {
            base.Initialize(model, dependencies);

            Subscribe(model.Path, OnPathUpdated);

            _navMeshAgent.speed = model.GetRealMoveSpeed();

            // var deltaTime = Time.deltaTime;
            // var impulse = unit.DesiredPosition.Value - unit.CurrentPosition;
            // var delta = deltaTime * unit.GetRealMoveSpeed() * impulse.normalized;
            // unit.ThisTransform.Value.Translate(delta);
        }

        private void OnPathUpdated(NavMeshPath path)
        {
            if (Model.HasPath == false)
            {
                _navMeshAgent.isStopped = true;
                return;
            }

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetPath(path);
        }
    }
}
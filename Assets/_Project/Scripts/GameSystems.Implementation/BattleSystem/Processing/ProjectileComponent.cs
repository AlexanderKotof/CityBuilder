using System;
using System.Threading.Tasks;
using CityBuilder.Configs.Scriptable.Battle;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Processing
{
    public class ProjectileComponent : MonoBehaviour
    {
        private UniTaskCompletionSource _taskCompletionSource;
        private IBattleUnit _shooter;
        private IBattleUnit _target;
        private ProjectileConfigSo _config;
        private Vector3 _targetPosition;

        public void Init(IBattleUnit shooter, IBattleUnit target, ProjectileConfigSo config)
        {
            _taskCompletionSource = new();
            _shooter = shooter;
            _target = target;
            _targetPosition = Vector3.zero;
            _config = config;

            transform.position = shooter.CurrentPosition + new Vector3(0, shooter.Config.Size.y);
            transform.rotation = Quaternion.LookRotation(target.CurrentPosition - transform.position);
        }
        
        public void Init(IBattleUnit shooter, Vector3 targetPosition, ProjectileConfigSo config)
        {
            _taskCompletionSource = new();
            _shooter = shooter;
            _target = null;
            _targetPosition = targetPosition;
            _config = config;

            transform.position = shooter.CurrentPosition + new Vector3(0, shooter.Config.Size.y);
            transform.rotation = Quaternion.LookRotation(_targetPosition - transform.position);
        }

        public void Tick(float dt)
        {
            var direction = _target != null ?
                _target.CurrentPosition - transform.position :
                _targetPosition - transform.position;
            if (direction.sqrMagnitude >
                _config.DistanceThreshold * _config.DistanceThreshold)
            {
                //TODO: add ballistics, etc..
                var velocity = dt * _config.ProjectileSpeed * direction;
                transform.Translate(velocity);
            }
            else
            {
                _taskCompletionSource.TrySetResult();
            }
        }

        public UniTask Hit()
        {
            return _taskCompletionSource?.Task ?? UniTask.CompletedTask;
        }
    }
}
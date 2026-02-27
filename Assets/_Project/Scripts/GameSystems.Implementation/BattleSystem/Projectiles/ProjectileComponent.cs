using System;
using System.Threading.Tasks;
using CityBuilder.Configs.Scriptable.Battle;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Projectiles
{
    public class ProjectileComponent : MonoBehaviour
    {
        private UniTaskCompletionSource _taskCompletionSource;
        private IBattleUnit _shooter;
        private IBattleUnit? _target;
        private ProjectileConfigSo _config;
        private Vector3 _targetPosition;

        public void Init(IBattleUnit shooter, IBattleUnit target, ProjectileConfigSo config)
        {
            _taskCompletionSource = new();
            _shooter = shooter;
            _target = target;
            _targetPosition = target.CurrentPosition;
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
            TryUpdateTargetPosition();
            
            if (_target != null && _target.IsAlive == false)
            {
                _taskCompletionSource.TrySetResult();
                return;
            }

            var direction = _targetPosition - transform.position;
            if (direction.sqrMagnitude > _config.DistanceThreshold * _config.DistanceThreshold)
            {
                //TODO: add ballistics, etc..
                var velocity = dt * _config.ProjectileSpeed * direction.normalized;
                transform.position += velocity;
                transform.rotation = Quaternion.LookRotation(velocity);
            }
            else
            {
                _taskCompletionSource.TrySetResult();
            }
        }

        private void TryUpdateTargetPosition()
        {
            if (_target == null) return;
            _targetPosition = _target.CurrentPosition + new Vector3(0, _target.Config.Size.y * 0.5f);
        }

        public UniTask Hit()
        {
            return _taskCompletionSource?.Task ?? UniTask.CompletedTask;
        }
    }
}
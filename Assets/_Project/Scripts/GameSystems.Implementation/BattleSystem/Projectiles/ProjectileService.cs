using System;
using System.Collections.Generic;
using CityBuilder.GameSystems.Common.ViewSystem;
using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Projectiles
{
    public class ProjectileService : IInitializable, IDisposable, ITickable
    {
        private readonly ViewsCollectionController<ProjectileComponent> _projectilesViews;
        
        private readonly List<ProjectileModel> _modelsToRemove = new();

        public ProjectileService(IViewsProvider viewsProvider)
        {
            _projectilesViews = new ViewsCollectionController<ProjectileComponent>(viewsProvider);
        }
        
        public void Initialize()
        {
 
        }

        public void Dispose()
        {
            _projectilesViews.Dispose();
            _modelsToRemove.Clear();
        }
        
        /// <summary>
        /// Shoot directional projectile
        /// </summary>
        /// <param name="shooter"></param>
        /// <param name="target"></param>
        /// <param name="hitCallback"></param>
        public async UniTaskVoid ShootProjectile(IBattleUnit shooter, IBattleUnit target, Action hitCallback)
        {
            var config = shooter.Config.ProjectileConfig;
            var model = new ProjectileModel(shooter, target);
            var view = await _projectilesViews.AddView(config.ProjectileAssetKey, model);
            view.Init(shooter, target, config);
            await view.Hit();
            hitCallback?.Invoke();
            _modelsToRemove.Add(model);
        }
        
        /// <summary>
        /// Shoot projectile to position
        /// </summary>
        /// <param name="shooter"></param>
        /// <param name="position"></param>
        /// <param name="hitCallback"></param>
        public async UniTaskVoid ShootProjectile(IBattleUnit shooter, Vector2 position, Action hitCallback)
        {
            var config = shooter.Config.ProjectileConfig;
            var model = new ProjectileModel(shooter, null);
            var view = await _projectilesViews.AddView(config.ProjectileAssetKey, model);
            view.Init(shooter, position, config);
            await view.Hit();
            hitCallback?.Invoke();
            _modelsToRemove.Add(model);
        }

        public void Tick()
        {
            float dt = Time.deltaTime;
            foreach (var view in _projectilesViews.ActiveViews)
            {
                view.Tick(dt);
            }

            foreach (var model in _modelsToRemove)
            {
                _projectilesViews.Return(model);
            }
            _modelsToRemove.Clear();
        }
    }
}
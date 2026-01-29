using System;
using CityBuilder.Dependencies;
using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem;
using GameSystems.Common.ViewSystem.ViewsProvider;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using Views.Implementation.BattleSystem;
using ViewSystem;

namespace GameSystems.Implementation.BattleSystem
{
    // ТЗ Боевая система:
    // см папку с ГДД
    public class BattleFeature : IInitializable, IDisposable, ITickable
    {
        private readonly BattleManager _battleManager;
        private readonly BattleSystemModel _battleSystemModel;

        private readonly ViewsCollectionController<BattleUnitBaseView> _playerUnitsViewsCollection;
        private readonly ViewsCollectionController<BattleUnitBaseView> _enemiesUnitsViewsCollection;
        
        private readonly PlayerBuildingsUnitsController _playerBuildingsUnitsController;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public BattleFeature(BattleManager battleManager, PlayerBuildingsUnitsController battleUnitsController, IViewsProvider viewsProvider, IViewWithModelProvider viewWithModelProvider, BattleSystemModel battleSystemModel)
        {
            _battleManager = battleManager;
            _playerBuildingsUnitsController = battleUnitsController;
            _battleSystemModel = battleSystemModel;

            var parentGo = new GameObject("--- Battle Units ---").transform;

            // var container = new DependencyContainer();
            // container.Register(viewWithModelProvider);
            
            _playerUnitsViewsCollection = new ViewsCollectionController<BattleUnitBaseView>(viewsProvider, defaultParent: parentGo.transform);
            _enemiesUnitsViewsCollection = new ViewsCollectionController<BattleUnitBaseView>(viewsProvider, defaultParent: parentGo.transform);

            _playerUnitsViewsCollection.AddTo(_disposable);
            _enemiesUnitsViewsCollection.AddTo(_disposable);
        }

        public void Initialize()
        {
            SubscribePlayerUnits();

            SubscribeEnemiesUnits();
            
            _playerBuildingsUnitsController.Init();
        }

        public void Dispose()
        {
            _playerBuildingsUnitsController.Deinit();
            
            _disposable.Dispose();
        }

        public void Tick()
        {
            _battleManager.Update();
        }
       
        private void SubscribePlayerUnits()
        {
            _battleSystemModel.PlayerUnits
                .SubscribeToCollection((data) => OnAddPlayerUnit(data).Forget(), OnRemovePlayerUnit)
                .AddTo(_disposable);
            async UniTaskVoid OnAddPlayerUnit(BattleUnitBase unit)
            {
                var view = await _playerUnitsViewsCollection.AddView(unit.Config.AssetKey, unit);
                view.Initialize(unit);
                var transform = view.ThisTransform;
                unit.ThisTransform.Set(transform);
                transform.position = unit.StartPosition.Value;
            }
            void OnRemovePlayerUnit(BattleUnitBase unit)
            {
                _playerUnitsViewsCollection.Recycle(unit);
            }
        }
        
        private void SubscribeEnemiesUnits()
        {
            _battleSystemModel.Enemies
                .SubscribeToCollection((data) => OnAddEnemyUnit(data).Forget(), OnRemoveEnemyUnit)
                .AddTo(_disposable);
            async UniTaskVoid OnAddEnemyUnit(BattleUnitBase unit)
            {
                var view = await _enemiesUnitsViewsCollection.AddView(unit.Config.AssetKey, unit);
                view.Initialize(unit);
                var transform = view.ThisTransform;
                unit.ThisTransform.Set(transform);
                transform.position = unit.StartPosition.Value;
            }
            void OnRemoveEnemyUnit(BattleUnitBase unit)
            {
                _enemiesUnitsViewsCollection.Recycle(unit);
            }
        }
    }
}
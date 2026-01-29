using System;
using CityBuilder.Dependencies;
using GameSystems.Common.ViewSystem.ViewsProvider;
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

        private readonly BattleUnitsViewsCollection _playerUnitsViewsCollection;
        private readonly BattleUnitsViewsCollection _enemiesUnitsViewsCollection;
        
        private readonly PlayerBuildingsUnitsController _playerBuildingsUnitsController;

        public BattleFeature(BattleManager battleManager, PlayerBuildingsUnitsController battleUnitsController, IViewWithModelProvider viewWithModelProvider, BattleSystemModel battleSystemModel)
        {
            _battleManager = battleManager;
            _playerBuildingsUnitsController = battleUnitsController;
            _battleSystemModel = battleSystemModel;

            var parentGo = new GameObject("--- Battle Units ---").transform;

            var container = new DependencyContainer();
            container.Register(viewWithModelProvider);
            
            //TODO: create inner feature dependencies container
            //TODO: refactoring pretendent
            _playerUnitsViewsCollection = new BattleUnitsViewsCollection(
                _battleSystemModel.PlayerUnits,
                container,
                parentGo);
            _enemiesUnitsViewsCollection = new BattleUnitsViewsCollection(
                _battleSystemModel.Enemies,
                container,
                parentGo);
        }

        public void Initialize()
        {
            _playerUnitsViewsCollection.Initialize();
            _enemiesUnitsViewsCollection.Initialize();
            _playerBuildingsUnitsController.Init();
        }

        public void Dispose()
        {
            _playerBuildingsUnitsController.Deinit();
            _playerUnitsViewsCollection.Deinit();
            _enemiesUnitsViewsCollection.Deinit();
        }

        public void Tick()
        {
            _battleManager.Update();
        }
    }
}
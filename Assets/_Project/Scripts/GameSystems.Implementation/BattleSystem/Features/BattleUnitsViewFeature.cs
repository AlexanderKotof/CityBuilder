using System;
using CityBuilder.GameSystems.Common.ViewSystem;
using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;
using CityBuilder.Utilities.Extensions;
using CityBuilder.Views.Implementation.BattleSystem;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Features
{
    /// <summary>
    /// Фича для управления вью юнитов для боевки
    /// </summary>
    public class BattleUnitsViewFeature : IInitializable, IDisposable
    {
        private readonly BattleManager _battleManager;
        private readonly BattleSystemModel _battleSystemModel;

        private readonly ViewsCollectionController<BattleUnitBaseView> _playerUnitsViewsCollection;
        private readonly ViewsCollectionController<BattleUnitBaseView> _enemiesUnitsViewsCollection;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public BattleUnitsViewFeature(BattleManager battleManager, IViewsProvider viewsProvider, BattleSystemModel battleSystemModel)
        {
            _battleManager = battleManager;
            _battleSystemModel = battleSystemModel;

            var parentGo = new GameObject("--- Battle Units ---").transform;
            _playerUnitsViewsCollection = new ViewsCollectionController<BattleUnitBaseView>(viewsProvider, defaultParent: parentGo.transform);
            _enemiesUnitsViewsCollection = new ViewsCollectionController<BattleUnitBaseView>(viewsProvider, defaultParent: parentGo.transform);

            _playerUnitsViewsCollection.AddTo(_disposable);
            _enemiesUnitsViewsCollection.AddTo(_disposable);
        }

        public void Initialize()
        {
            SubscribePlayerUnits();

            SubscribeEnemiesUnits();
        }

        public void Dispose()
        {
            _disposable.Dispose();
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
                unit.ThisTransform.Value = (transform);
                transform.position = unit.StartPosition.Value;
            }
            void OnRemovePlayerUnit(BattleUnitBase unit)
            {
                _playerUnitsViewsCollection.Return(unit);
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
                unit.ThisTransform.Value = (transform);
                transform.position = unit.StartPosition.Value;
            }
            void OnRemoveEnemyUnit(BattleUnitBase unit)
            {
                _enemiesUnitsViewsCollection.Return(unit);
            }
        }
    }
}
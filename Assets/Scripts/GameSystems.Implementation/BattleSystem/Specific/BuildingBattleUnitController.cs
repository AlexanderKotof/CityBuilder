using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem;
using GameSystems.Implementation.BuildingSystem.Domain;
using UniRx;
using UnityEngine;

namespace GameSystems.Implementation.BattleSystem
{
    /// <summary>
    /// Контроллирует сущьность боевого юнита, приатаченного к зданию
    /// </summary>
    public class BuildingBattleUnitController
    {
        private readonly ViewsCollectionController<BattleUnitUIComponent> _viewsController;
        private readonly BuildingModel _building;
        private readonly BattleUnitsFactory _factory;
        private readonly BattleSystemModel _battleSystemModel;
        private readonly BuildingsModel _buildingsModel;
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        
        private BattleUnitBase _currentUnit;
        private BattleUnitUIComponent _uiView;
        
        public BattleUnitBase Unit => _currentUnit;
        public BuildingModel Building => _building;

        public BuildingBattleUnitController(
            ViewsCollectionController<BattleUnitUIComponent> viewsController,
            BuildingModel building,
            BattleUnitsFactory factory,
            BattleSystemModel battleSystemModel,
            BuildingsModel buildingsModel)
        {
            _viewsController = viewsController;
            _building = building;
            _factory = factory;
            _battleSystemModel = battleSystemModel;
            _buildingsModel = buildingsModel;
        }

        public void Initialize()
        {
            SetBattleUnit();
        }
        
        private void SetBattleUnit()
        {
            _subscriptions.Clear();

            _currentUnit = _factory.CreateBattleUnitFromBuilding(_building);
            _battleSystemModel.AddPlayerBuilding(_currentUnit);
            
            _currentUnit.OnDiedObservable.Subscribe(OnBuildingUnitDestroyed).AddTo(_subscriptions);
            _building.Level.Subscribe(OnBuildingLevelUpdated).AddTo(_subscriptions);
            
            if (_buildingsModel.MainBuilding == Building)
            {
                _battleSystemModel.SetMainBuilding(_currentUnit);
            }
            
            OnAddBuildingBattleUnit(_currentUnit).Forget();
        }

        private void OnBuildingLevelUpdated(int _)
        {
            _factory.TryLevelUpBuildingUnit(_building, _currentUnit);
        }

        private void OnBuildingUnitDestroyed(BattleUnitBase _)
        {
            _subscriptions.Dispose();
            
            Debug.Log($"Building {Building.BuildingName} destroyed!");
                
            //TODO: This is other service responsibility how to control building destroy process
            _buildingsModel.RemoveBuilding(Building);
                
            if (_buildingsModel.MainBuilding == Building)
            {
                Debug.LogError("Destroyed main building battle model... GAME OVER");
            }
        }
        
        public void Dispose()
        {
            if (_currentUnit != null)
            {
                RemovePlayerBuildingUnit(_currentUnit);
                _battleSystemModel.RemoveUnit(_currentUnit);
            }
            
            _subscriptions.Dispose();
        }
        
        async UniTaskVoid OnAddBuildingBattleUnit(BattleUnitBase unit)
        {
            _uiView = await _viewsController.AddView(unit, unit.ThisTransform.Value);
            _uiView.Init(unit);
            
            unit.ThisTransform.Subscribe(OnTransformUpdated).AddTo(_subscriptions);
            OnTransformUpdated(unit.ThisTransform.Value);
            return;
                
            void OnTransformUpdated(Transform value)
            {
                if (value == null)
                    return;
                    
                //Debug.LogError($"Transform updated for {unit.Config.name}!", value);
                    
                _uiView.transform.SetParent(value);
                _uiView.transform.localPosition = Vector3.up;
            }
        }
            
        void RemovePlayerBuildingUnit(BattleUnitBase unit)
        {
            _viewsController.Return(unit);
            _subscriptions.Clear();
        }
    }
}
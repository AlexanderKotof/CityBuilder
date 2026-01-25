using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using Configs;
using Configs.Schemes.BattleSystem;
using UnityEngine;
using Views.Implementation.BattleSystem;

namespace GameSystems.Implementation.BattleSystem
{
    // ТЗ Боевая система:
    // см папку с ГДД
    
    public class BattleFeature : GameSystemBase, IUpdateGamSystem
    {
        public BattleManager BattleManager { get; }
        public BattleSystemModel BattleSystemModel { get; } = new();

        private readonly BattleUnitsViewsCollection _playerUnitsViewsCollection;
        private readonly BattleUnitsViewsCollection _enemiesUnitsViewsCollection;
        
        private readonly PlayerBuildingsUnitsController _playerBuildingsUnitsController;

        public BattleFeature(IDependencyContainer container) : base(container)
        {
            var settings = container.Resolve<GameConfigProvider>().GetConfig<BattleUnitsConfigScheme>();
            var buildingsModel = container.Resolve<BuildingsModel>();
            BattleManager = new BattleManager(BattleSystemModel, settings);
            _playerBuildingsUnitsController =
                new PlayerBuildingsUnitsController(BattleSystemModel, settings, buildingsModel, container);
            
            var parentGo = new GameObject("--- Battle Units ---").transform;
            
            //TODO: create inner feature dependencies container
            _playerUnitsViewsCollection = new BattleUnitsViewsCollection(
                BattleSystemModel.PlayerUnits,
                container,
                parentGo);
            _enemiesUnitsViewsCollection = new BattleUnitsViewsCollection(
                BattleSystemModel.Enemies,
                container,
                parentGo);
            

        }

        public override Task Init()
        {
            _playerUnitsViewsCollection.Initialize();
            _enemiesUnitsViewsCollection.Initialize();
            _playerBuildingsUnitsController.Init();
            return Task.CompletedTask;         
        }

        public override Task Deinit()
        {
            _playerBuildingsUnitsController.Deinit();
            _playerUnitsViewsCollection.Deinit();
            _enemiesUnitsViewsCollection.Deinit();
            return Task.CompletedTask;
        }

        public void Update()
        {
            BattleManager.Update();
        }
    }
}
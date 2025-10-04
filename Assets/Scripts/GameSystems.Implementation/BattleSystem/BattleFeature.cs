using System.Threading.Tasks;
using CityBuilder.Dependencies;
using Configs;
using Configs.Schemes.BattleSystem;
using UnityEngine;
using Views.Implementation.BattleSystem;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleFeature : GameSystemBase
    {
        public BattleManager BattleManager { get; }
        public BattleUnitsModel BattleUnitsModel { get; } = new();

        private readonly BattleUnitsViewsCollection _playerUnitsViewsCollection;
        private readonly BattleUnitsViewsCollection _enemiesUnitsViewsCollection;
        private readonly BattleUnitsConfigScheme _settings;

        public BattleFeature(IDependencyContainer container) : base(container)
        {
            _settings = container.Resolve<GameConfigProvider>().GetConfig<BattleUnitsConfigScheme>();
            
            BattleManager = new BattleManager(BattleUnitsModel);
            
            var parentGo = new GameObject(" --- Battle Units --- ").transform;
            
            //TODO: create inner feature dependencies container
            _playerUnitsViewsCollection = new BattleUnitsViewsCollection(
                BattleUnitsModel.PlayerUnits,
                container,
                parentGo);
            _enemiesUnitsViewsCollection = new BattleUnitsViewsCollection(
                BattleUnitsModel.Enemies,
                container,
                parentGo);
        }

        public override Task Init()
        {
            _playerUnitsViewsCollection.Initialize();
            _enemiesUnitsViewsCollection.Initialize();
            return Task.CompletedTask;         
        }

        public override Task Deinit()
        {
            _playerUnitsViewsCollection.Deinit();
            _enemiesUnitsViewsCollection.Deinit();
            return Task.CompletedTask;
        }
    }
}
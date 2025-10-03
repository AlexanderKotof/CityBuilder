using System.Threading.Tasks;
using CityBuilder.Dependencies;
using Configs;
using Configs.Schemes.BattleSystem;
using UnityEngine;
using Views.Implementation.BattleSystem;
using ViewSystem;

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
            
            var viewsProvider = container.Resolve<IViewsProvider>();
            var viewsWithModelProvider = new ViewWithModelProvider(viewsProvider, container);
            var parentGo = new GameObject(" --- Battle Units --- ").transform;

            _playerUnitsViewsCollection = new BattleUnitsViewsCollection(
                BattleUnitsModel.PlayerUnits,
                viewsWithModelProvider,
                parentGo);
            _enemiesUnitsViewsCollection = new BattleUnitsViewsCollection(
                BattleUnitsModel.Enemies,
                viewsWithModelProvider,
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
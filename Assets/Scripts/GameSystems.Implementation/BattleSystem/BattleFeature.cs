using System.Threading.Tasks;
using CityBuilder.Dependencies;
using Configs;
using Configs.Schemes.BattleSystem;
using UnityEngine;
using Views.Implementation.BattleSystem;

namespace GameSystems.Implementation.BattleSystem
{
    // ТЗ Боевая система:
    
    // BattleUnit - может перемещаться, атаковать, может быть убит, может атаковать здания
    // Имеет показатели: здоровье, урон, скорость, защита, дальность атаки
    
    // Игрок может производить юнитов в специальных зданиях (плюс есть здания где они могут размещаться)
    
    // Вражеские юниты могут разрушать здания игрока (их можно отстроить)
    
    // Башни - являются юнитом? - да, но особым
    
    //TODO: HealthComponent logic
    
    public class BattleFeature : GameSystemBase, IUpdateGamSystem
    {
        public BattleManager BattleManager { get; }
        public BattleUnitsModel BattleUnitsModel { get; } = new();

        private readonly BattleUnitsViewsCollection _playerUnitsViewsCollection;
        private readonly BattleUnitsViewsCollection _enemiesUnitsViewsCollection;

        public BattleFeature(IDependencyContainer container) : base(container)
        {
            var settings = container.Resolve<GameConfigProvider>().GetConfig<BattleUnitsConfigScheme>();
            BattleManager = new BattleManager(BattleUnitsModel, settings);
            
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

        public void Update()
        {
            BattleManager.Update();
        }
    }
}
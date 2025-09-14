using System.Collections.Generic;
using System.Linq;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Reactive;
using GameSystems;

namespace PeopleFeature
{
    public class PopulationModel
    {
        public ReactiveProperty<int> CurrentPopulation { get; } = new();
        public ReactiveProperty<int> AvailableHouseholds { get; } = new();

        public PopulationModel(int startingPopulation, int startingHouseholds)
        {
            CurrentPopulation.Value = startingPopulation;
            AvailableHouseholds.Value = startingHouseholds;
        }

        public void OnWeekChanged()
        {

        }

        public void UpdateAvailableHouseholds(int value) => AvailableHouseholds.Set(value);

}

    public record AvailableHouseholdIncreaseUnit(HouseHoldsIncreaseBuildingFunction Function,  BuildingModel Building)
    {
        public HouseHoldsIncreaseBuildingFunction Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
        
        public int GetHouseholdIncreaseValue()
        {
            return Function.AvailableHouseholdsIncrease; // + _buildingLevel * additional value
        }
    }
    
    public class PopulationFeature : GameSystemBase
    {
        private const int StartingPopulation = 100;
        private const int StartingHouseholds = 100;
        
        
        private readonly BuildingsModel _buildingsModel;
        public PopulationModel PopulationModel { get; }
        
        private readonly Dictionary<BuildingModel, AvailableHouseholdIncreaseUnit> _increaseHousesUnits = new();
        private readonly GameTimeSystem.GameTimeSystem _gameTimeSystem;

        public PopulationFeature(IDependencyContainer container) : base(container)
        {
            PopulationModel = new PopulationModel(StartingPopulation, StartingHouseholds);
            _buildingsModel = container.Resolve<BuildingsModel>();
            _gameTimeSystem = container.Resolve<GameTimeSystem.GameTimeSystem>();
        }

        public override void Init()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);

            _gameTimeSystem.NewDayStarted += OnNewDayStarted;
        }
        
        public override void Deinit()
        {
            _gameTimeSystem.NewDayStarted -= OnNewDayStarted;
            
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
        
        private void OnNewDayStarted(int day)
        {
            throw new System.NotImplementedException();
        }
        
        private void OnBuildingAdded(BuildingModel building)
        {
            if (building.Config.TryGetHouseholdsCapacityFunction(out HouseHoldsIncreaseBuildingFunction householdsIncrease) == false)
            {
                return;
            }

            var increaseUnit = new AvailableHouseholdIncreaseUnit(householdsIncrease, building);
            _increaseHousesUnits.Add(building, increaseUnit);
            
            building.Level.AddListener(OnBuildingLevelUpdated);

            UpdateAvailableHouseholds();
        }
        
        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_increaseHousesUnits.ContainsKey(building))
            {
                return;
            }

            _increaseHousesUnits.Remove(building);
            
            building.Level.RemoveListener(OnBuildingLevelUpdated);

            UpdateAvailableHouseholds();
        }

        private void OnBuildingLevelUpdated(int _) => UpdateAvailableHouseholds();
        
        private void UpdateAvailableHouseholds()
        {
            int households = StartingHouseholds;
            foreach (var unit in _increaseHousesUnits.Values)
            {
                households += unit.GetHouseholdIncreaseValue();
            }
            
            PopulationModel.UpdateAvailableHouseholds(households);
        }
    }
}
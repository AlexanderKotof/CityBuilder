using System.Collections.Generic;
using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using Configs.Implementation.Buildings.Functions;
using GameSystems.Implementation.GameTimeSystem;

namespace GameSystems.Implementation.PopulationFeature
{
    public class PopulationFeature : GameSystemBase
    {
        private const int StartingPopulation = 100;
        private const int StartingHouseholds = 100;
        
        private readonly BuildingsModel _buildingsModel;
        public PopulationModel PopulationModel { get; }
        
        private readonly Dictionary<BuildingModel, AvailableHouseholdIncreaseUnit> _increaseHousesUnits = new();
        private readonly DateModel _dateModel;

        public PopulationFeature(IDependencyContainer container) : base(container)
        {
            PopulationModel = new PopulationModel(StartingPopulation, StartingHouseholds);
            _buildingsModel = container.Resolve<BuildingsModel>();
            _dateModel = container.Resolve<DateModel>();
        }

        public override Task Init()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);

            _dateModel.OnDayChanged += OnNewDayStarted;
            _dateModel.OnWeekChanged += OnWeekChanged;
            return Task.CompletedTask;
        }

        public override Task Deinit()
        {
            _dateModel.OnDayChanged -= OnNewDayStarted;
            _dateModel.OnWeekChanged -= OnWeekChanged;
            
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);
            return Task.CompletedTask;
        }
        
        private void OnWeekChanged()
        {
            PopulationModel.OnWeekChanged();
        }
        
        private void OnNewDayStarted()
        {
            PopulationModel.OnDayChanged();
        }
        
        private void OnBuildingAdded(BuildingModel building)
        {
            if (_increaseHousesUnits.ContainsKey(building))
            {
                return;
            }
            
            if (building.Config.TryGetHouseholdsCapacityFunction(out HouseHoldsIncreaseBuildingFunction householdsIncrease) == false)
            {
                return;
            }
            
            var increaseUnit = new AvailableHouseholdIncreaseUnit(householdsIncrease, building);
            _increaseHousesUnits.Add(building, increaseUnit);
            
            building.Level.Subscribe(OnBuildingLevelUpdated);

            UpdateAvailableHouseholds();
        }
        
        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_increaseHousesUnits.ContainsKey(building))
            {
                return;
            }

            _increaseHousesUnits.Remove(building);
            
            building.Level.Unsubscribe(OnBuildingLevelUpdated);

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
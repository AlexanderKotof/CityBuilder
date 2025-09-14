using System.Collections.Generic;
using System.Linq;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Reactive;
using GameSystems;
using GameTimeSystem;
using UnityEngine;

namespace PeopleFeature
{
    public class PopulationModel
    {
        public ReactiveProperty<int> CurrentPopulation { get; } = new();
        public ReactiveProperty<int> EmployedPopulation { get; } = new(0);
        public ReactiveProperty<int> AvailableHouseholds { get; } = new();

        private readonly float _dayGrowthFactor = 0.1f;
        private readonly float _dayGrowthProbability = 0.1f;
        
        private readonly float _dayDeathFactor = 0.1f;
        private readonly float _dayDeathProbability = 0.1f;
        
        private readonly int _weekGrowthBase = 10;
        
        



        public PopulationModel(int startingPopulation, int startingHouseholds)
        {
            CurrentPopulation.Value = startingPopulation;
            AvailableHouseholds.Value = startingHouseholds;
        }

        public void OnWeekChanged()
        {
            CurrentPopulation.Value =  CurrentPopulation.Value + _weekGrowthBase;
            Debug.Log($"New week starts! Current population: {CurrentPopulation.Value}");
        }
        
        public void OnDayChanged()
        {
            int growth = Random.value <= _dayGrowthProbability
                ? Mathf.FloorToInt(_dayGrowthFactor * CurrentPopulation.Value * Random.value)
                : 0;
            int died = Random.value <= _dayDeathProbability
                ? Mathf.FloorToInt(_dayDeathFactor * CurrentPopulation.Value * Random.value)
                : 0;

            int change = growth - died;

            if (change == 0)
            {
                return;
            }
            
            CurrentPopulation.Value = CurrentPopulation.Value + change;
            
            Debug.Log($"Population changed today by {change}, current value: {CurrentPopulation.Value}");
        }

        public void UpdateAvailableHouseholds(int value) => AvailableHouseholds.Set(value);
    }

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

        public override void Init()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);

            _dateModel.OnDayChanged += OnNewDayStarted;
            _dateModel.OnWeekChanged += OnWeekChanged;
        }

        public override void Deinit()
        {
            _dateModel.OnDayChanged -= OnNewDayStarted;
            _dateModel.OnWeekChanged -= OnWeekChanged;
            
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);
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
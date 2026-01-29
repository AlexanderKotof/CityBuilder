using System;
using System.Collections.Generic;
using BuildingSystem;
using BuildingSystem.Extensions;
using Configs.Scriptable.Buildings.Functions;
using GameSystems.Implementation.GameTime;
using VContainer.Unity;

namespace GameSystems.Implementation.PopulationFeature
{
    public class PopulationFeature : IInitializable, IDisposable
    {
        private readonly BuildingsModel _buildingsModel;
        private readonly PopulationModel _populationModel;
        private readonly DateModel _dateModel;
        private readonly Dictionary<BuildingModel, AvailableHouseholdIncreaseUnit> _increaseHousesUnits = new();

        public PopulationFeature(BuildingsModel buildingsModel, DateModel dateModel, PopulationModel populationModel)
        {
            _populationModel = populationModel;
            _buildingsModel = buildingsModel;
            _dateModel = dateModel;
        }

        public void Initialize()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);

            _dateModel.OnDayChanged += OnNewDayStarted;
            _dateModel.OnWeekChanged += OnWeekChanged;
        }

        public void Dispose()
        {
            _dateModel.OnDayChanged -= OnNewDayStarted;
            _dateModel.OnWeekChanged -= OnWeekChanged;
            
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);
        }
        
        private void OnWeekChanged()
        {
            _populationModel.OnWeekChanged();
        }
        
        private void OnNewDayStarted()
        {
            _populationModel.OnDayChanged();
        }
        
        private void OnBuildingAdded(BuildingModel building)
        {
            if (_increaseHousesUnits.ContainsKey(building))
            {
                return;
            }
            
            if (building.Config.TryGetHouseholdsCapacityFunction(out HouseHoldsIncreaseBuildingFunctionSo householdsIncrease) == false)
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
            int households = PopulationModel.StartingHouseholds;
            foreach (var unit in _increaseHousesUnits.Values)
            {
                households += unit.GetHouseholdIncreaseValue();
            }
            
            _populationModel.UpdateAvailableHouseholds(households);
        }
    }
}
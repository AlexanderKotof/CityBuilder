using System;
using System.Collections.Generic;
using CityBuilder.Configs.Scriptable.Buildings.Functions;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Extensions;
using CityBuilder.GameSystems.Implementation.GameTime;
using CityBuilder.Utilities.Extensions;
using UniRx;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.PopulationFeature
{
    public class PopulationFeature : IInitializable, IDisposable
    {
        private readonly BuildingsModel _buildingsModel;
        private readonly PopulationModel _populationModel;
        private readonly DateModel _dateModel;
        private readonly CompositeDisposable _subscriptions = new CompositeDisposable();
        private readonly Dictionary<BuildingModel, AvailableHouseholdIncreaseUnit> _increaseHousesUnits = new();

        public PopulationFeature(BuildingsModel buildingsModel, DateModel dateModel, PopulationModel populationModel)
        {
            _populationModel = populationModel;
            _buildingsModel = buildingsModel;
            _dateModel = dateModel;
        }

        public void Initialize()
        {
            _buildingsModel.Buildings.SubscribeToCollection(OnBuildingAdded, OnBuildingRemoved).AddTo(_subscriptions);
            _dateModel.OnDayChanged += OnNewDayStarted;
            _dateModel.OnWeekChanged += OnWeekChanged;
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
            _dateModel.OnDayChanged -= OnNewDayStarted;
            _dateModel.OnWeekChanged -= OnWeekChanged;
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

            building.Level.Subscribe(OnBuildingLevelUpdated).AddTo(_subscriptions);

            UpdateAvailableHouseholds();
        }
        
        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_increaseHousesUnits.ContainsKey(building))
            {
                return;
            }

            _increaseHousesUnits.Remove(building);
            
            //TODO:
            //building.Level.Unsubscribe(OnBuildingLevelUpdated);

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
using System;
using BuildingSystem;
using CityBuilder.Grid;
using Configs.Scriptable.Buildings;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation
{
    public class MergeBuildingsFeature : IInitializable, IDisposable
    {
        private readonly BuildingManager _manager;
        private readonly BuildingsModel _model;
        private readonly MergeFeatureConfigurationSo _configuration;

        public MergeBuildingsFeature(BuildingManager manager, BuildingsModel model, MergeFeatureConfigurationSo configuration)
        {
            _manager = manager;
            _model = model;
            _configuration = configuration;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
        
        public bool TryMergeBuildingsFromTo(BuildingModel fromBuilding, BuildingModel toBuilding)
        {
            if (CanLevelUpMerge(toBuilding, fromBuilding))
            {
                _model.RemoveBuilding(fromBuilding);
                
                toBuilding.IncreaseLevel();
                
                Debug.Log($"Building level upgraded to {toBuilding.Level}");

                return true;
            }

            if (CanRecipeMerge(toBuilding, fromBuilding))
            {           
                //TODO: implement merge mechanics
            }

            return false;
        }

        private bool CanRecipeMerge(BuildingModel toBuilding, BuildingModel fromBuilding)
        {
            return false;
        }

        private bool CanLevelUpMerge(BuildingModel second, BuildingModel first)
        {
            return
                Equals(first.Config, second.Config) &&
                first.Level.Value == second.Level.Value;
        }
    }
}
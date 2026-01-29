using System;
using BuildingSystem;
using CityBuilder.Grid;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation
{
    public class MergeBuildingsFeature : IInitializable, IDisposable
    {
        private readonly BuildingManager _manager;
        private readonly BuildingsModel _model;

        public MergeBuildingsFeature(BuildingManager manager, BuildingsModel model)
        {
            _manager = manager;
            _model = model;
        }

        public void Initialize()
        {
        }

        public void Dispose()
        {
        }
        
        public bool TryMergeBuildingsFromTo(CellModel from, CellModel to)
        {
            if (!_manager.TryDragCellFromTo(from, to))
            {
                return false;
            }
            
            if (!_manager.TryGetBuilding(from, out var fromBuilding) || !_manager.TryGetBuilding(to, out var toBuilding))
            {
                return false;
            }

            //TODO: implement merge mechanics
            
            if (CanBeUpgraded(toBuilding, fromBuilding))
            {
                _manager.RemoveBuilding(from);
                
                toBuilding.IncreaseLevel();
                
                Debug.Log($"Building level upgraded to {toBuilding.Level}");

                return true;
            }

            return false;
        }

        private bool CanBeUpgraded(BuildingModel toBuilding, BuildingModel fromBuilding)
        {
            return true;
        }
    }
}
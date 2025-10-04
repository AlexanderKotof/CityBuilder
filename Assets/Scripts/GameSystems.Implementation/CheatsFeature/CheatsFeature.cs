using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using GameSystems.Implementation.GameInteractionFeature;
using UnityEngine;

namespace GameSystems.Implementation.CheatsFeature
{
    public class CheatsFeature : GameSystemBase, IUpdateGamSystem
    {
        private readonly Raycaster _raycaster;
        private readonly BuildingManager _buildingManager;

        private readonly Dictionary<KeyCode, int> _placeBuildingsKeys = new()
        {
            { KeyCode.Alpha1, 0 },
            { KeyCode.Alpha2, 1 },
            { KeyCode.Alpha3, 2 },
            { KeyCode.Alpha4, 3 },
            { KeyCode.Alpha5, 4 },
            { KeyCode.Alpha6, 5 },
            { KeyCode.Alpha7, 6 },
            { KeyCode.Alpha8, 7 },
            { KeyCode.Alpha9, 8 },
            { KeyCode.Alpha0, 9 },
        };
        
        public CheatsFeature(IDependencyContainer container) : base(container)
        {
            _raycaster = container.Resolve<Raycaster>();
            _buildingManager = container.Resolve<BuildingManager>();
        }

        public void Update()
        {
            foreach (var keyKodeToIndex in _placeBuildingsKeys)
            {
                if (Input.GetKeyDown(keyKodeToIndex.Key) &&
                    _raycaster.TryGetCellFromScreenPoint(Input.mousePosition, out var cell))
                {
                    _buildingManager.TryPlaceBuilding(cell, keyKodeToIndex.Value);
                }
            }
        }
    }
}
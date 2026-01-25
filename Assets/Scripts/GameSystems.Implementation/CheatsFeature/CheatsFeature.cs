using System;
using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using GameSystems.Implementation.BattleSystem;
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
        
        private readonly BattleManager _battleManager;

        private readonly Guid _defaultPlayerUnitGuid;
        private readonly Guid _defaultEnemyUnitGuid;


        public CheatsFeature(IDependencyContainer container) : base(container)
        {
            _raycaster = container.Resolve<Raycaster>();
            _buildingManager = container.Resolve<BuildingManager>();
            _battleManager = container.Resolve<BattleManager>();

            _ = Guid.TryParse("14e80a78-6faa-416b-949d-ea277530c2d5", out _defaultPlayerUnitGuid);
            _ = Guid.TryParse("af712df2-2896-4cd6-8085-1aba3d1d2f31", out _defaultEnemyUnitGuid);
        }

        public void Update()
        {
            
            //Buidings
            foreach (var keyKodeToIndex in _placeBuildingsKeys)
            {
                if (Input.GetKeyDown(keyKodeToIndex.Key) &&
                    _raycaster.TryGetCellFromScreenPoint(Input.mousePosition, out var cell))
                {
                    _buildingManager.TryPlaceBuilding(cell, keyKodeToIndex.Value);
                }
            }

            
            //Encounters
            if (Input.GetKeyDown(KeyCode.E))
            {
                _battleManager.InvasionBegins(new InvasionData()
                {
                    Invaders = new()
                    {
                        (_defaultEnemyUnitGuid, 3),
                    }
                });
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                _battleManager.PlayerUnitCreate(new List<Guid>() { _defaultPlayerUnitGuid });
            }
        }
    }
}
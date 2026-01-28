using System;
using System.Collections.Generic;
using System.Linq;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using Configs.Scriptable;
using GameSystems.Implementation.BattleSystem;
using GameSystems.Implementation.GameInteractionFeature;
using UnityEngine;
using VContainer.Unity;

namespace GameSystems.Implementation.CheatsFeature
{
    public class CheatsFeature : ITickable
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
        private readonly BattleUnitsConfigSO _battleUnitsConfigSo;

        private readonly BattleUnitConfigSO _defaultPlayerUnitGuid;
        private readonly BattleUnitConfigSO _defaultEnemyUnitGuid;


        public CheatsFeature(Raycaster raycaster, BuildingManager buildingManager, BattleManager battleManager, BattleUnitsConfigSO battleUnitsConfigSO)
        {
            _raycaster = raycaster;
            _buildingManager = buildingManager;
            _battleManager = battleManager;
            _battleUnitsConfigSo = battleUnitsConfigSO;

            _defaultPlayerUnitGuid = battleUnitsConfigSO.PlayerUnitsConfigs.FirstOrDefault();
            _defaultEnemyUnitGuid = battleUnitsConfigSO.EnemiesConfigs.FirstOrDefault();
        }

        public void Tick()
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
                _battleManager.PlayerUnitCreate(new List<BattleUnitConfigSO>() { _defaultPlayerUnitGuid });
            }
        }
    }
}
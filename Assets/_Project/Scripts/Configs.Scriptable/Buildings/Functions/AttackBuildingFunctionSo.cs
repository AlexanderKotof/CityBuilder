using CityBuilder.Configs.Scriptable.Battle;
using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings.Functions
{
    [CreateAssetMenu(fileName = nameof(AttackBuildingFunctionSo), menuName = ConfigsMenuName.BuildingFunctionsMenuName + nameof(AttackBuildingFunctionSo))]
    public class AttackBuildingFunctionSo : BuildingFunctionSo
    {
        public BattleUnitConfigSO[] BattleUnitPerLevel;
    }
}
using System;
using Unity.Collections;
using UnityEngine;

namespace Configs.Scriptable.Battle
{
    [CreateAssetMenu(fileName = nameof(BattleUnitConfigSO), menuName = ConfigsMenuName.BattleMenuName + nameof(BattleUnitConfigSO))]
    public class BattleUnitConfigSO : ScriptableObject, IConfigBase
    {
        public string Name = "Unit";
        public string InternalDescription = "Some battle unit";
        public float Health = 100;
        public float Damage = 5;
        public float AttackRange = 1;
        public float Defense = 0;
        public float MoveSpeed = 1;
        public float AttackSpeed = 1;
        public string AssetKey = "Unit";
        public AttackPossibilityAndPriority AttackPossibilityAndPriority = 0;
        
        [field: SerializeField, ReadOnly]
        public string Id { get; private set; } = Guid.NewGuid().ToString();
    }
}
using System;
using CityBuilder.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CityBuilder.Configs.Scriptable.Battle
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
        public Vector2 Size = Vector2.one * 0.3f;
        public AttackPossibilityAndPriority AttackPossibilityAndPriority = 0;
        
        public ProjectileConfigSo ProjectileConfig;
        
        [field: SerializeField, ReadOnly]
        public string Id { get; private set; }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
        }
    }
}
using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Battle
{
    [CreateAssetMenu(fileName = nameof(ProjectileConfigSo), menuName = ConfigsMenuName.BattleMenuName + nameof(ProjectileConfigSo))]
    public class ProjectileConfigSo : ScriptableObject, IConfigBase
    {
        public string ProjectileAssetKey = "Projectile";
        public float ProjectileSpeed = 1;
        public float DistanceThreshold = 0.2f;

    }
}
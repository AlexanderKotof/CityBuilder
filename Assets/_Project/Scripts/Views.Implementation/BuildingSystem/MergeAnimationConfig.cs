using LitMotion;
using UnityEngine;

namespace CityBuilder.Views.Implementation.BuildingSystem
{
    public class MergeAnimationConfig : ScriptableObject
    {
        [field: SerializeField]
        public float ScalingDelay { get; private set; }
        [field: SerializeField]
        public float ScalingDuration{ get; private set; }
        [field: SerializeField]
        public Ease ScalingEase{ get; private set; }

        [field: SerializeField]
        public float MovingDelay{ get; private set; }
        
        [field: SerializeField]
        public float MovingDuration{ get; private set; }
        
        [field: SerializeField]
        public Ease MovingEase{ get; private set; }
    }
}
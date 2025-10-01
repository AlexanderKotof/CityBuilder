using System;
using UnityEngine;

namespace ResourcesSystem
{
    [Serializable]
    public class ResourceConfig
    {
        [field: SerializeField]
        public ResourceType Type { get; set; }
        [field: SerializeField]
        public int Amount { get; set; }

        public override string ToString()
        {
            return $"{Type.ToString()}x{Amount.ToString()}";
        }
    }

    public enum ResourceType
    {
        Wood,
        Rock,
        Metal,
        Food,
        Gold,
    }
}
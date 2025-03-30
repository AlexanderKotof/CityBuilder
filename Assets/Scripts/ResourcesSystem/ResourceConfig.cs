using System;
using UnityEngine;

namespace ResourcesSystem
{
    [Serializable]
    public class ResourceConfig
    {
        [field: SerializeField]
        public ResourceType Type { get; private set; }
        [field: SerializeField]
        public int Amount { get; private set; }
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
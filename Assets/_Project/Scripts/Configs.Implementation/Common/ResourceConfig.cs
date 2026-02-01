using System;
using UnityEngine;

namespace CityBuilder.Configs.Implementation.Common
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
}
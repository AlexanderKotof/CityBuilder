using Configs.Implementation.Common;
using Configs.Scriptable;
using UnityEngine;

namespace ResourcesSystem
{
    public class PlayerResourcesModel : ResourcesStorageModel
    {
        public PlayerResourcesModel(ResourcesDefaultConfigurationSo settings) : base(settings._defaultCapacity)
        {
        }

        public override void AddResource(ResourceConfig resource)
        {
            base.AddResource(resource);
            Debug.Log($"[{nameof(ResourcesStorageModel)}] Added {resource.ToString()} to player");
        }

        public override void RemoveResource(ResourceModel resource)
        {
            base.RemoveResource(resource);
            Debug.Log($"[{nameof(ResourcesStorageModel)}] Removed {resource.ToString()} from player");
        }
    }
}
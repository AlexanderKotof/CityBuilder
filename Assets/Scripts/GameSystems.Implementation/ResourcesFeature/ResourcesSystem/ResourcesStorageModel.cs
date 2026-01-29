using Configs.Implementation.Common;
using UnityEngine;

namespace ResourcesSystem
{
    public class ResourcesStorageModel : ResourcesAmounts
    {
        public int Capacity { get; private set; } = 0;

        public int DefaultCapacity { get; }

        public int Fill { get; private set; } = 0;

        public ResourcesStorageModel(int capacity)
        {
            Capacity = capacity;
            DefaultCapacity = capacity;
        }

        public override void AddResource(ResourceModel resource)
        {
            base.AddResource(resource);
            Fill += resource.Amount.Value;
        }

        public override void AddResource(ResourceConfig resource)
        {
            base.AddResource(resource);
            Fill += resource.Amount;
        }

        public override void RemoveResource(ResourceModel resource)
        {
            base.RemoveResource(resource);
            Fill -= resource.Amount.Value;
        }

        public override bool CanAddResource(ResourceModel resource)
        {
            return Capacity >= Fill + resource.Amount.Value;
        }

        public void UpdateCapacity(int newCapacity)
        {
            Capacity = newCapacity;
        }
    }
}
using System;
using System.Collections.Generic;
using CityBuilder.Reactive;

namespace ResourcesSystem
{
    public class PlayerResourcesModel : ResourcesStorageModel
    {
        public PlayerResourcesModel(int capacity) : base(capacity)
        {
        }
    }

    public class ResourcesStorageModel : ResourcesAmounts
    {
        public int Capacity { get; private set; } = 0;
        
        public int Fill { get; private set; } = 0;

        public ResourcesStorageModel(int capacity)
        {
            Capacity = capacity;
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
    
    public class ResourcesAmounts
    {
        private ReactiveDictionary<ResourceType, ResourceModel> ResourcesMap { get; } = new();
        
        public IEnumerable<ResourceModel> Resources => ResourcesMap.Values;
        
        public event Action<ResourceModel, int>? AmountUpdated;
        
        public virtual void AddResource(ResourceModel resource)
        {
            if (ResourcesMap.TryGetValue(resource.Id, out var existingResource))
            {
                existingResource.AddAmount(resource.Amount.Value);
            }
            else
            {
                ResourcesMap.Add(resource.Id, resource);
            }
            
            AmountUpdated?.Invoke(resource, resource.Amount.Value);
        }
        
        public virtual void AddResource(ResourceConfig resource)
        {
            if (ResourcesMap.TryGetValue(resource.Type, out var existingResource))
            {
                existingResource.AddAmount(resource.Amount);
                AmountUpdated?.Invoke(existingResource, resource.Amount);
            }
            else
            {
                var resourceModel = new ResourceModel(resource);
                ResourcesMap.Add(resource.Type, resourceModel);
                AmountUpdated?.Invoke(resourceModel, resource.Amount);
            }
        }
        
        public virtual void RemoveResource(ResourceModel resource)
        {
            if (ResourcesMap.TryGetValue(resource.Id, out var existingResource))
            {
                existingResource.RemoveAmount(resource.Amount);
                AmountUpdated?.Invoke(resource, resource.Amount);
            }
        }

        public virtual bool CanAddResource(ResourceModel resource) => true;
        
        public int GetResourceAmount(ResourceType resourceType)
        {
            return ResourcesMap.TryGetValue(resourceType, out var existingResource) ? existingResource.Amount.Value : 0;
        }
        
        public void AddResources(ResourcesAmounts resources)
        {
            foreach (var resource in resources.ResourcesMap.Values)
            {
                AddResource(resource);
            }
        }
        
        public void RemoveResources(ResourcesAmounts resources)
        {
            foreach (var resource in resources.ResourcesMap.Values)
            {
                RemoveResource(resource);
            }
        }

        public bool HasResource(ResourceModel resource)
        {
            if (ResourcesMap.TryGetValue(resource.Id, out var existingResource))
            {
                return existingResource.HasAmount(resource.Amount);
            }

            return false;
        }
        
        public bool HasResources(ResourcesAmounts resources)
        {
            foreach (var resource in resources.ResourcesMap.Values)
            {
                if (HasResource(resource) == false)
                {
                    return false;
                }
            }

            return true;
        }
        
        public void Clear()
        {
            ResourcesMap.Clear();
        }

    }
}
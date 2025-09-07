using System;
using System.Collections.Generic;
using CityBuilder.Reactive;

namespace ResourcesSystem
{
    public class PlayerResourcesModel : ResourcesModel
    {
        
    }
    
    public class ResourcesModel
    {
        public ReactiveDictionary<ResourceType, ResourceModel> ResourcesMap { get; } = new();
        
        public IEnumerable<ResourceModel> Resources => ResourcesMap.Values;

        public event Action<ResourceModel, int>? AmountUpdated;
        
        public void AddResource(ResourceModel resource)
        {
            if (ResourcesMap.TryGetValue(resource.Id, out var existingResource))
            {
                existingResource.AddAmount(resource.Amount);
            }
            else
            {
                ResourcesMap.Add(resource.Id, resource);
            }
            
            AmountUpdated?.Invoke(resource, resource.Amount);
        }
        
        public void AddResource(ResourceConfig resource)
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
        
        public int GetResourceAmount(ResourceType resourceType)
        {
            return ResourcesMap.TryGetValue(resourceType, out var existingResource) ? existingResource.Amount.Value : 0;
        }
        
        public void AddResources(ResourcesModel resources)
        {
            foreach (var resource in resources.ResourcesMap.Values)
            {
                AddResource(resource);
            }
        }
        
        public void RemoveResource(ResourceModel resource)
        {
            if (ResourcesMap.TryGetValue(resource.Id, out var existingResource))
            {
                existingResource.RemoveAmount(resource.Amount);
                AmountUpdated?.Invoke(resource, resource.Amount);
            }
        }
        
        public void RemoveResources(ResourcesModel resources)
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
        
        public bool HasResources(ResourcesModel resources)
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
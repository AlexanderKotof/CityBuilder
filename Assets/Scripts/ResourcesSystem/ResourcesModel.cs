using System;
using CityBuilder.Reactive;

namespace ResourcesSystem
{
    public class ResourcesModel
    {
        public ReactiveDictionary<ResourceType, Resource> Resources { get; } = new();

        public event Action<Resource, int>? AmountUpdated;

        public void AddResource(Resource resource)
        {
            if (Resources.TryGetValue(resource.Id, out var existingResource))
            {
                existingResource.AddAmount(resource.Amount);
            }
            else
            {
                Resources.Add(resource.Id, resource);
            }
            
            AmountUpdated?.Invoke(resource, resource.Amount);
        }
        
        public void RemoveResource(Resource resource)
        {
            if (Resources.TryGetValue(resource.Id, out var existingResource))
            {
                existingResource.RemoveAmount(resource.Amount);
                AmountUpdated?.Invoke(resource, resource.Amount);
            }
        }

        public bool HasResource(Resource resource)
        {
            if (Resources.TryGetValue(resource.Id, out var existingResource))
            {
                return existingResource.HasAmount(resource.Amount);
            }

            return false;
        }
    }
}
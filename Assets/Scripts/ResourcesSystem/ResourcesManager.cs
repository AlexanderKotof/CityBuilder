using Configs.Schemes;

namespace ResourcesSystem
{
    public class ResourcesManager
    {
        public PlayerResourcesModel PlayerResourcesStorage { get; }

        public ResourcesManager(ResourcesDefaultConfigurationScheme resourcesConfiguration)
        {
            PlayerResourcesStorage = new(resourcesConfiguration.DefaultCapacity);
            
            foreach (var resourceConfig in resourcesConfiguration.Resources)
            {
                PlayerResourcesStorage.AddResource(new ResourceModel(resourceConfig));
            }
        }
    }
}
namespace ResourcesSystem
{
    public class ResourcesManager
    {
        public PlayerResourcesModel PlayerResources { get; } = new();

        public ResourcesManager(ResourcesConfigurationSO resourcesConfiguration)
        {
            foreach (var resourceConfig in resourcesConfiguration.Resources)
            {
                PlayerResources.AddResource(new ResourceModel(resourceConfig));
            }
        }
    }
}
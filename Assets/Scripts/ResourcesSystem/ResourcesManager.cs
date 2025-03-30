namespace ResourcesSystem
{
    public class ResourcesManager
    {
        public ResourcesModel ResourcesModel { get; } = new();

        public ResourcesManager(ResourcesConfigurationSO resourcesConfiguration)
        {
            foreach (var resourceConfig in resourcesConfiguration.Resources)
            {
                ResourcesModel.AddResource(new Resource(resourceConfig));
            }
        }
    }
}
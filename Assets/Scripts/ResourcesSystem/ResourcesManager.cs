using Configs.Schemes;
using Configs.Scriptable;
using VContainer.Unity;

namespace ResourcesSystem
{
    public class ResourcesManager
    {
        public ResourcesManager(PlayerResourcesModel model, ResourcesDefaultConfigurationSO resourcesConfiguration)
        {
            foreach (var resourceConfig in resourcesConfiguration.StartResources)
            {
                model.AddResource(new ResourceModel(resourceConfig));
            }
        }
    }
}
using Configs.Scriptable;
using VContainer.Unity;

namespace ResourcesSystem
{
    public class ResourcesManager : IInitializable
    {
        private readonly PlayerResourcesModel _model;
        private readonly ResourcesDefaultConfigurationSO _resourcesConfiguration;

        public ResourcesManager(PlayerResourcesModel model, ResourcesDefaultConfigurationSO resourcesConfiguration)
        {
            _model = model;
            _resourcesConfiguration = resourcesConfiguration;
        }

        public void Initialize()
        {
            foreach (var resourceConfig in _resourcesConfiguration.StartResources)
            {
                _model.AddResource(new ResourceModel(resourceConfig));
            }
        }
    }
}
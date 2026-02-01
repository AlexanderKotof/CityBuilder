using CityBuilder.Configs.Scriptable;
using CityBuilder.GameSystems.Implementation.ResourcesFeature.Core.Domain;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.ResourcesFeature.Core
{
    public class ResourcesManager : IInitializable
    {
        private readonly PlayerResourcesModel _model;
        private readonly ResourcesDefaultConfigurationSo _resourcesConfiguration;

        public ResourcesManager(PlayerResourcesModel model, ResourcesDefaultConfigurationSo resourcesConfiguration)
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
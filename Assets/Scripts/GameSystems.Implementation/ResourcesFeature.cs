using CityBuilder.Dependencies;
using Configs;
using Configs.Schemes;
using ResourcesSystem;

namespace GameSystems.Implementation
{
    public class ResourcesFeature : GameSystemBase
    {
        public ResourcesManager ResourcesManager { get; }

        public PlayerResourcesModel PlayerResourcesModel => ResourcesManager.PlayerResourcesStorage;
    
        public ResourcesFeature(IDependencyContainer container) : base(container)
        {
            var resourcesConfig = Container.Resolve<GameConfigProvider>().GetConfig<ResourcesDefaultConfigurationScheme>();
            ResourcesManager = new ResourcesManager(resourcesConfig);
        }
    }
}
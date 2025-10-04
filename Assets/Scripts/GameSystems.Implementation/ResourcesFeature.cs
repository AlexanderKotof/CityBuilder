using CityBuilder.Dependencies;
using ResourcesSystem;

namespace GameSystems.Implementation
{
    public class ResourcesFeature : GameSystemBase
    {
        public ResourcesManager ResourcesManager { get; }

        public PlayerResourcesModel PlayerResourcesModel => ResourcesManager.PlayerResourcesStorage;
    
        public ResourcesFeature(IDependencyContainer container) : base(container)
        {
            ResourcesManager = new ResourcesManager(container.Resolve<GameConfigurationSo>().ResourcesConfig);
        }
    }
}
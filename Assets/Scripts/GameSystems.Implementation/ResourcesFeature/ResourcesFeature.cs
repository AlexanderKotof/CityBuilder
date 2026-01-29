using Configs.Scriptable;
using ResourcesSystem;

namespace GameSystems.Implementation
{
    public class ResourcesFeature
    {
        public ResourcesManager ResourcesManager { get; }
        
        public ResourcesFeature(ResourcesDefaultConfigurationSo resourcesDefaultConfiguration)
        {
            //ResourcesManager = new ResourcesManager(resourcesDefaultConfiguration);
        }
    }
}
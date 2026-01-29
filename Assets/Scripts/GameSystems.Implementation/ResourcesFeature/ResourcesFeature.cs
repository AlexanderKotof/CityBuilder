using Configs.Scriptable;
using ResourcesSystem;

namespace GameSystems.Implementation
{
    public class ResourcesFeature
    {
        public ResourcesManager ResourcesManager { get; }
        
        public ResourcesFeature(ResourcesDefaultConfigurationSO resourcesDefaultConfiguration)
        {
            //ResourcesManager = new ResourcesManager(resourcesDefaultConfiguration);
        }
    }
}
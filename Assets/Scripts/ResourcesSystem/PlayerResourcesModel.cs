using Configs.Scriptable;

namespace ResourcesSystem
{
    public class PlayerResourcesModel : ResourcesStorageModel
    {
        public PlayerResourcesModel(ResourcesDefaultConfigurationSO settings) : base(settings.DefaultCapacity)
        {
        }
    }
}
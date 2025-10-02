using ResourcesSystem;

namespace Configs.Implementation.Buildings.Functions
{
    public class ResourceProductionBuildingFunction : BuildingFunction
    {
        public ResourceConfig[] RequireResourcesForProduction { get; set;}
        
        public ResourceConfig[] ProduceResourcesByTick { get; set;}
    }
}
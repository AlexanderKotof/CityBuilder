using ResourcesSystem;

namespace CityBuilder.BuildingSystem
{
    public class ResourceProductionBuildingFunction : BuildingFunction
    {
        public ResourceConfig[] RequireResourcesForProduction { get; set;}
        
        public ResourceConfig[] ProduceResourcesByTick { get; set;}
    }
}
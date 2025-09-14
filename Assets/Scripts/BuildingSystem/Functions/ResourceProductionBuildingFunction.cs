using ResourcesSystem;

namespace CityBuilder.BuildingSystem
{
    public class ResourceProductionBuildingFunction : BuildingFunction
    {
        public ResourceConfig[] RequireResourcesForProduction;
        
        public ResourceConfig[] ProduceResourcesByTick;
    }
}
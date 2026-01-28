using Configs.Scriptable;
using ResourcesSystem;

namespace Configs.Implementation.Buildings.Functions
{
    public class ResourceProductionBuildingFunctionSO : BuildingFunctionSO
    {
        public ResourceConfig[] RequireResourcesForProduction;

        public ResourceConfig[] ProduceResourcesByTick;
    }
}
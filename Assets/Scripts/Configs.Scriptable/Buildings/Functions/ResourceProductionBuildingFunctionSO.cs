using Configs.Implementation.Common;
using UnityEngine.Serialization;

namespace Configs.Scriptable.Buildings.Functions
{
    public class ResourceProductionBuildingFunctionSo : BuildingFunctionSo
    {
        [FormerlySerializedAs("RequireResourcesForProduction")]
        public ResourceConfig[] _requireResourcesForProduction;

        [FormerlySerializedAs("ProduceResourcesByTick")]
        public ResourceConfig[] _produceResourcesByTick;
    }
}
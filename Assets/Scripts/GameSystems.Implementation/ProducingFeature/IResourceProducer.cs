using System.Collections.Generic;
using ResourcesSystem;

namespace GameSystems.Implementation.ProducingFeature
{
    public interface IResourceProducer
    {
        bool CanProduce();
        IEnumerable<ResourceConfig> GetCosts();
        IEnumerable<ResourceConfig> GetProduction();
    }
}
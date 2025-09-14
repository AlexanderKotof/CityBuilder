using System.Collections.Generic;
using ResourcesSystem;

namespace ProducingFeature
{
    public interface IResourceProducer
    {
        bool CanProduce();
        IEnumerable<ResourceConfig> GetCosts();
        IEnumerable<ResourceConfig> GetProduction();
    }
}
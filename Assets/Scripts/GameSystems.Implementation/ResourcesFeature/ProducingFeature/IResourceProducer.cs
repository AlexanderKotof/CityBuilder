using System.Collections.Generic;
using Configs.Implementation.Common;
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
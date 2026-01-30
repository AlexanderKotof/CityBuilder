using System.Collections.Generic;
using Configs.Implementation.Common;
using ResourcesSystem;

namespace GameSystems.Implementation.ProducingFeature
{
    public interface IResourceProducer
    {
        //SHOULD CHECK EMPLOYEES, RESOURCES etc
        bool CanProduce();
        IEnumerable<ResourceConfig> GetCosts();
        IEnumerable<ResourceConfig> GetProduction();
    }
}
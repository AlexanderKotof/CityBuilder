using System.Collections.Generic;
using CityBuilder.Configs.Implementation.Common;

namespace CityBuilder.GameSystems.Implementation.ResourcesFeature.ProducingFeature
{
    public interface IResourceProducer
    {
        //SHOULD CHECK EMPLOYEES, RESOURCES etc
        bool CanProduce();
        IEnumerable<ResourceConfig> GetCosts();
        IEnumerable<ResourceConfig> GetProduction();
    }
}
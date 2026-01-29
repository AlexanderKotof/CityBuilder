using CityBuilder.Grid;
using Configs.Scriptable.Buildings;

namespace BuildingSystem
{
    public class BuildingFactory
    {
        public BuildingModel Create(BuildingConfigSo config, CellModel cellModel)
        {
            var building = new BuildingModel(1, config);
            return building;
        }
    }
}

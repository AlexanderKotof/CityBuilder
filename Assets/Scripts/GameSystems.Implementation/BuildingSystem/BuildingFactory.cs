using CityBuilder.Grid;
using Configs.Scriptable.Buildings;
using GameSystems.Implementation.BuildingSystem.Domain;

namespace GameSystems.Implementation.BuildingSystem
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

using CityBuilder.Grid;
using Configs.Scriptable.Buildings;
using GameSystems.Implementation.BuildingSystem.Domain;

namespace GameSystems.Implementation.BuildingSystem
{
    public class BuildingFactory
    {
        public BuildingModel Create(BuildingConfigSo config, CellModel cellModel)
        {
            var building = new BuildingModel(0, config);
            return building;
        }

        public BuildingModel Create(MergeBuildingsRecipeSo recipe, CellModel cellModel)
        {
            return new BuildingModel(0, recipe.Product);
        }
    }
}

using CityBuilder.Configs.Scriptable.Buildings;
using CityBuilder.Configs.Scriptable.Buildings.Merge;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;

namespace CityBuilder.GameSystems.Implementation.BuildingSystem
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

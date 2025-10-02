using Configs.Schemes;

namespace Configs.Implementation.Buildings.Functions
{
    public abstract class BuildingFunction : ConfigBase, IBuildingFunction
    {
        public string Note { get; set; } = "some note";
    }
}
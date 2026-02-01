using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings.Functions
{
    public abstract class BuildingFunctionSo : ScriptableObject, IBuildingFunction, IConfigBase
    {
        [TextArea(2, 4)]
        public string Note = "some note";
    }
}
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs.Scriptable.Buildings
{
    public abstract class BuildingFunctionSo : ScriptableObject, IBuildingFunction, IConfigBase
    {
        [TextArea(2, 4)]
        public string Note = "some note";
    }
}
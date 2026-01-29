using UnityEngine;
using UnityEngine.Serialization;

namespace Configs.Scriptable.Buildings
{
    public abstract class BuildingFunctionSo : ScriptableObject, IBuildingFunction, IConfigBase
    {
        public string Note = "some note";
    }
}
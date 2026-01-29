using UnityEngine;
using UnityEngine.Serialization;

namespace Configs.Scriptable.Buildings
{
    public abstract class BuildingFunctionSo : ScriptableObject, IBuildingFunction, IConfigBase
    {
        [FormerlySerializedAs("Note")]
        public string _note = "some note";
    }
}
using Configs.Implementation.Buildings.Functions;
using UnityEngine;

namespace Configs.Scriptable
{
    public abstract class BuildingFunctionSO : ScriptableObject, IBuildingFunction, IConfigBase
    {
        public string Note = "some note";
    }
}
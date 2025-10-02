using System;
using Configs.Schemes;
using UnityEngine;

namespace CityBuilder.BuildingSystem
{
    public interface IBuildingFunction
    {

    }

    public abstract class BuildingFunction : ConfigBase, IBuildingFunction
    {
    }
}
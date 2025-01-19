using CityBuilder.BuildingSystem;
using UnityEngine;

namespace ViewSystem
{
    public class BuildingWindow : WindowBase<Building>
    {
        public override void Initialize(Building model)
        {
            base.Initialize(model);
            
            Debug.Log("Initialized building window");
        }
    }
}
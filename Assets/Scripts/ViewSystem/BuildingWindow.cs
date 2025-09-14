using CityBuilder.BuildingSystem;
using UnityEngine;

namespace ViewSystem
{
    public class BuildingWindow : WindowBase<BuildingModel>
    {
        public override void Initialize(BuildingModel model)
        {
            base.Initialize(model);
            
            Debug.Log("Initialized building window");
        }
    }
}
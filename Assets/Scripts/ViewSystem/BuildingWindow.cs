using CityBuilder.BuildingSystem;
using UnityEngine;

namespace ViewSystem
{
    public class BuildingWindowView : WindowViewBase<BuildingModel>
    {
        //public override string AssetId => "BuildingModel";
        
        public override void Initialize(BuildingModel model)
        {
            base.Initialize(model);
            
            Debug.Log("Initialized building window");
        }
    }
}
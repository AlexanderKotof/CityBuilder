using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using UnityEngine;

namespace ViewSystem
{
    public class BuildingWindowView : WindowViewBase<BuildingModel>
    {
        //public override string AssetId => "BuildingModel";
        
        public override void Initialize(BuildingModel model, IDependencyContainer dependencies)
        {
            base.Initialize(model, dependencies);
            
            Debug.Log("Initialized building window");
        }
    }
}
using CityBuilder.Dependencies;
using CityBuilder.GameSystems.Common.WindowSystem.Window;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace CityBuilder.Views.Implementation.Windows
{
    public class BuildingInfoWindowView : WindowViewBase<BuildingInfoWindowModel>
    {
        public TextMeshProUGUI SelectedBuildingName;
        
        public override void Initialize(BuildingInfoWindowModel model, IDependencyContainer dependencies)
        {
            base.Initialize(model, dependencies);
            
            Debug.Log("Initialized building window");
            
            Subscribe(model.SelectedBuilding, OnSelectedBuilding);
        }

        private void OnSelectedBuilding([CanBeNull] BuildingModel selected)
        {
            SelectedBuildingName.SetText(Model.SelectedBuilding.Value?.BuildingName ?? string.Empty);
        }
    }
}
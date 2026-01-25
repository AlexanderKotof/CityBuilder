using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using TMPro;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BuildingSystem
{
    public class BuildingView : ViewWithModel<BuildingModel>
    {
        //TODO: add additional functionality

        public Canvas UICanvas;
        
        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;

        public override void Initialize(BuildingModel model, IDependencyContainer container)
        {
            base.Initialize(model, container);
            
            Subscribe(model.Level, (value) => LevelIndicator.SetText($"Lvl {value}"));
            Subscribe(model.WorldPosition, SetWorldPosition);
            NameText.SetText(model.BuildingName);

            SetUiActive(false);
        }

        private void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetUiActive(bool value)
        {
            UICanvas.enabled = value;
        }
    }
}
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using TMPro;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BuildingSystem
{
    public class BuildingView : ViewWithModel<BuildingModel>
    {
        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;

        public override void Initialize(BuildingModel model, IDependencyContainer container)
        {
            base.Initialize(model, container);
            
            Subscribe(model.Level, (value) => LevelIndicator.SetText($"Lvl {value}"));
            Subscribe(model.WorldPosition, SetWorldPosition);
            NameText.SetText(model.BuildingName);
        }

        private void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
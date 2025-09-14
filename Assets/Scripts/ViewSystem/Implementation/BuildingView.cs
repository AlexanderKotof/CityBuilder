using System.Linq;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Reactive;
using TMPro;
using UnityEngine;

namespace ViewSystem.Implementation
{
    public class BuildingView : ViewWithModel<BuildingModel>
    {
        //public override string AssetId { get; } = "BuildingView";

        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;

        public override void Initialize(BuildingModel model)
        {
            base.Initialize(model);
            
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
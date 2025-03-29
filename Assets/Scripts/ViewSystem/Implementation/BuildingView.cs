using System.Linq;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Reactive;
using TMPro;
using UnityEngine;

namespace ViewSystem.Implementation
{
    public class BuildingView : ViewWithModel<Building>
    {
        //public override string AssetId { get; } = "BuildingView";

        public TextMeshProUGUI LevelIndicator;

        public override void Initialize(Building model)
        {
            base.Initialize(model);
            
            Subscribe(model.Level, (value) => LevelIndicator.SetText($"Lvl {value}"));
            Subscribe(model.WorldPosition, SetWorldPosition);
        }

        private void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
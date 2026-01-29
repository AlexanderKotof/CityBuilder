using BuildingSystem;
using CityBuilder.Dependencies;
using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem.View;
using TMPro;
using UniRx;
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

            Initialize(model);
        }
        
        public void Initialize(BuildingModel model)
        {
            model.Level.AsObservable().Subscribe((value) => LevelIndicator.SetText($"Lvl {value}"));
            model.WorldPosition.AsObservable().Subscribe(SetWorldPosition).AddTo(this);
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
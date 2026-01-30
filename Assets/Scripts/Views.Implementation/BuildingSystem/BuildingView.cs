using GameSystems.Common.ViewSystem.View;
using GameSystems.Implementation.BuildingSystem.Domain;
using TMPro;
using UniRx;
using UnityEngine;

namespace Views.Implementation.BuildingSystem
{
    public class BuildingView : ViewWithModel<BuildingModel>
    {
        public Canvas UICanvas;
        
        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;
        
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
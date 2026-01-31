using System.Threading.Tasks;
using GameSystems.Common.ViewSystem.View;
using GameSystems.Implementation.BuildingSystem.Domain;
using TMPro;
using UniRx;
using UnityEngine;

namespace Views.Implementation.BuildingSystem
{
    public class BuildingView : ViewWithModel<BuildingModel>
    {
        public GameObject[] _visualsByLevel;
        
        public Canvas UICanvas;
        
        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;
        
        public void Initialize(BuildingModel model)
        {
            model.Level.Subscribe(OnLevelUpdated).AddTo(this);
            model.WorldPosition.Subscribe(SetWorldPosition).AddTo(this);
            
            NameText.SetText(model.BuildingName);

            SetUiActive(false);
        }

        private void OnLevelUpdated(int value)
        {
            LevelIndicator.SetText($"Lvl {(value + 1).ToString()}");
            
            if (_visualsByLevel == null || _visualsByLevel.Length == 0)
                return;
            
            for (var index = 0; index < _visualsByLevel.Length; index++)
            {
                _visualsByLevel[index].SetActive(index == value);
            }
        }

        private void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetUiActive(bool value)
        {
            UICanvas.enabled = value;
        }

        public async UniTask MergeTo(Vector3 toPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}
using TMPro;
using UnityEngine;

namespace CityBuilder.Views.Implementation.BuildingSystem
{
    public class BuildingWorldCanvas : MonoBehaviour
    {
        public Canvas UICanvas;
        
        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;

        public void SetName(string name)
        {
            NameText.SetText(name);
        }
        public void SetLevel(int level)
        {
            LevelIndicator.SetText($"Lvl {(level + 1).ToString()}");
        }

        public void SetUiActive(bool value)
        {
            UICanvas.enabled = value;
        }
    }
}
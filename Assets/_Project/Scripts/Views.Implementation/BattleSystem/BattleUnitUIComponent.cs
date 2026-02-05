using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder.Views.Implementation.BattleSystem
{
    public class BattleUnitUIComponent : MonoBehaviour
    {
        public Canvas canvas;
        public Slider healthSlider;
        public TextMeshProUGUI healthText;
        
        public Vector2 MaxHealthBarSize = new Vector2(100f, 10f);
        public Vector2 MinHealthBarSize = new Vector2(30f, 10f);
        
        private IBattleUnit _battleUnit;

        public void Init(IBattleUnit battleUnit)
        {
            _battleUnit = battleUnit;
            UpdateSize();
        }

        private void UpdateSize()
        {
            const float maxSize = 1000f;
            
            var startHealth = _battleUnit.Health.StartValue.Value;
            var rectTransform = (RectTransform)healthSlider.transform;
            
            if (startHealth > maxSize)
            {
                rectTransform.sizeDelta = MaxHealthBarSize;
            }
            else
            {
                rectTransform.sizeDelta = Vector2.Lerp(MinHealthBarSize, MaxHealthBarSize, startHealth / maxSize);
            }
        }

        private void Update()
        {
            if (_battleUnit == null)
                return;

            if (_battleUnit.Health.IsFull)
            {
                canvas.enabled = false;
                return;
            }

            var percent = _battleUnit.Health.CurrentValue / _battleUnit.Health.StartValue;
            healthSlider.value = percent;
            canvas.enabled = true;
            healthText.text = _battleUnit.Health.CurrentValue + "/" + _battleUnit.Health.StartValue;
        }
    }
}
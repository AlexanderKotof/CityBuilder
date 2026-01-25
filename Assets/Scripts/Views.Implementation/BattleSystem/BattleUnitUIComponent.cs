using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleUnitUIComponent : MonoBehaviour
    {
        public Canvas canvas;
        public Slider healthSlider;
        public TextMeshProUGUI healthText;
        
        private IBattleUnit _battleUnit;

        public void Init(IBattleUnit battleUnit)
        {
            _battleUnit = battleUnit;
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
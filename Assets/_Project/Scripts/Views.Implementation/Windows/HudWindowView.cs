using CityBuilder.Dependencies;
using CityBuilder.GameSystems.Common.WindowSystem.Window;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder.Views.Implementation.Windows
{
    public class HudWindowView : WindowViewBase<HudWindowModel>
    {
        [SerializeField]
        private Image _dayProgress;

        [SerializeField]
        private TextMeshProUGUI _timeText;
        
        public override void Initialize(HudWindowModel model, IDependencyContainer dependencies)
        {
            base.Initialize(model, dependencies);
            
            Subscribe(model.DayProgress, p => _dayProgress.fillAmount = p);
            Subscribe(model.Date, _timeText.SetText);
        }
    }
}
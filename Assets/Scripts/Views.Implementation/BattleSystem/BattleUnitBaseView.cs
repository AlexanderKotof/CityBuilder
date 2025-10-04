using CityBuilder.Dependencies;
using GameSystems.Implementation.BattleSystem;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BattleSystem
{
    public class BattleUnitBaseView : ViewWithModel<BattleUnitModel>
    {
        public Transform ThisTransform;
        
        [CanBeNull] public BattleUnitNavigationComponent NavigationComponent;
        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;

        public override void Initialize(BattleUnitModel model, IDependencyContainer container)
        {
            base.Initialize(model, container);
            
            model.ThisTransform.Set(ThisTransform);
            
            InitView(NavigationComponent, model);

            // Subscribe(model.Level, (value) => LevelIndicator.SetText($"Lvl {value}"));
            // Subscribe(model.WorldPosition, SetWorldPosition);
            // NameText.SetText(model.BuildingName);
        }
        
        private void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
using CityBuilder.Dependencies;
using GameSystems.Common.ViewSystem.View;
using GameSystems.Implementation.BattleSystem;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BattleSystem
{
    public class BattleUnitBaseView : ViewWithModel<BattleUnitBase>
    {
        public Transform ThisTransform;
        
        [CanBeNull] public BattleUnitNavigationComponent NavigationComponent;
        public TextMeshProUGUI LevelIndicator;
        public TextMeshProUGUI NameText;
        
        public BattleUnitUIComponent UIComponent;

        public override void Initialize(BattleUnitBase model, IDependencyContainer container)
        {
            base.Initialize(model, container);
            
            Debug.LogError("Unit view initialized");
            
            model.ThisTransform.Value = (ThisTransform);
            
            InitView(NavigationComponent, model);

            if (UIComponent != null) 
                UIComponent.Init(model);

            // Subscribe(model.Level, (value) => LevelIndicator.SetText($"Lvl {value}"));
            // Subscribe(model.WorldPosition, SetWorldPosition);
            // NameText.SetText(model.BuildingName);
        }
        
        private void SetWorldPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Initialize(BattleUnitBase model)
        {
            model.ThisTransform.Value = (ThisTransform);
            InitView(NavigationComponent, model);
            
            if (UIComponent != null) 
                UIComponent.Init(model);
        }
    }
}
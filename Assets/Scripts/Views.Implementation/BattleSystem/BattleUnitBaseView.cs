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
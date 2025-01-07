using BuildingSystem;
using TMPro;

namespace ViewSystem.Implementation
{
    public class BuildingView : ViewWithModel<Building>
    {
        public override string AssetId { get; } = "BuildingView";

        public TextMeshProUGUI LevelIndicator;

        public override void Initialize(Building model)
        {
            base.Initialize(model);
            
            model.Level.AddListener((value) => LevelIndicator.SetText(value.ToString()));
        }
    }
}
using CityBuilder.BuildingSystem;
using CityBuilder.Reactive;

namespace ViewSystem
{
    public class BuildingInfoWindowModel : IWindowViewModel
    {
        public ReactiveProperty<bool> IsActive { get; } = new();
        public ReactiveCommand Close { get; } = new();
        
        
        public readonly ReactiveProperty<BuildingModel?> SelectedBuilding = new();
    }
}
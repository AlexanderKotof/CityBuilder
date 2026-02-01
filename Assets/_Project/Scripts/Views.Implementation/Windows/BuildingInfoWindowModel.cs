using CityBuilder.GameSystems.Common.WindowSystem;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using UniRx;

namespace CityBuilder.Views.Implementation.Windows
{
    public class BuildingInfoWindowModel : IWindowViewModel
    {
        public ReactiveProperty<bool> IsActive { get; } = new();
        public ReactiveCommand Close { get; } = new();
        
        
        public readonly ReactiveProperty<BuildingModel?> SelectedBuilding = new();
        
        public void Dispose()
        {
        }
    }
}
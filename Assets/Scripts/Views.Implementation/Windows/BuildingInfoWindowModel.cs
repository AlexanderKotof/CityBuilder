using GameSystems.Common.WindowSystem;
using GameSystems.Implementation.BuildingSystem.Domain;
using UniRx;

namespace ViewSystem
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
using CityBuilder.GameSystems.Common.WindowSystem;
using UniRx;

namespace CityBuilder.Views.Implementation.Windows
{
    public class HudWindowModel : IWindowViewModel
    {
        public ReactiveProperty<bool> IsActive { get; } = new();
        public ReactiveCommand Close { get; } = new();
        public ReactiveProperty<float> DayProgress { get; } = new();
        public ReactiveProperty<string> Date { get; } = new();
        
        public ReactiveCommand ShowBuildWindowPressed { get; } = new();
        
        public ReactiveProperty<bool> IsInBattle { get; } = new();

        public void Dispose()
        {
            IsActive.Dispose();
            Close.Dispose();
            DayProgress.Dispose();
            Date.Dispose();
        }
    }
}
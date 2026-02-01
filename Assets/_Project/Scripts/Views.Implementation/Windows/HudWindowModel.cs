using CityBuilder.GameSystems.Common.WindowSystem;
using UniRx;

namespace CityBuilder.Views.Implementation.Windows
{
    public class HudWindowModel : IWindowViewModel
    {
        public ReactiveProperty<bool> IsActive { get; } = new();
        public ReactiveCommand Close { get; } = new();
        public ReactiveProperty<float> DayProgress { get; } = new();
        public ReactiveProperty<string> Date { get; }  = new();

        public void Dispose()
        {
            IsActive.Dispose();
            Close.Dispose();
        }
    }
}
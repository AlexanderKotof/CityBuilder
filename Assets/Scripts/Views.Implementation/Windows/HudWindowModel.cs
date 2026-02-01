using GameSystems.Common.WindowSystem;
using UniRx;

namespace ViewSystem
{
    public class HudWindowModel : IWindowViewModel
    {
        public ReactiveProperty<bool> IsActive { get; } = new();
        public ReactiveCommand Close { get; } = new();
        public ReactiveProperty<float> DayProgress { get; } = new();
        public void Dispose()
        {
            IsActive.Dispose();
            Close.Dispose();
        }
    }
}
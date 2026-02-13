using CityBuilder.GameSystems.Common.WindowSystem;
using UniRx;

namespace CityBuilder.Views.Implementation.Windows
{
    public class StartWindowModel : IWindowViewModel
    {
        public ReactiveProperty<bool> IsActive { get; } = new();
        public ReactiveCommand Close { get; } = new();
        
        public ReactiveProperty<bool> ShowRegistration { get; } = new();
        public ReactiveProperty<bool> ShowEnteringGame { get; } = new();
        
        public ReactiveCommand<string> RegistrationSubmit { get; } = new();
        public ReactiveCommand EnterGamePressed { get; } = new();
        
        public ReactiveProperty<string> PlayerNickname { get; } = new();
        
        public void Dispose()
        {
            IsActive.Dispose();
            Close.Dispose();
        }
    }
}
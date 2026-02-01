using System;
using CityBuilder.Dependencies;
using CityBuilder.GameSystems.Common.WindowSystem;
using CityBuilder.GameSystems.Implementation.GameTime;
using CityBuilder.Utilities.Extensions;
using CityBuilder.Views.Implementation.Windows;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.HudWindow
{
    public class HudWindowFeature : IInitializable, IDisposable
    {
        private readonly IWindowsProvider _windowsProvider;
        private readonly DateModel _dateModel;

        private readonly IDependencyContainer _innerDependencies = new DependencyContainer();
        
        private HudWindowModel _windowModel;
        private readonly CompositeDisposable _disposables = new();

        public HudWindowFeature(IWindowsProvider windowsProvider, DateModel dateModel)
        {
            _windowsProvider = windowsProvider;
            _dateModel = dateModel;

            //TODO: add inner dependencies
            
            //TODO: control all world-ui should be there or smth... 
        }

        public void Initialize()
        {
            InitializeWindow().Forget();
        }

        private async UniTaskVoid InitializeWindow()
        {
            _windowModel = await _windowsProvider.CreateWindow<HudWindowModel>(
                new WindowCreationData("HudWindow", 0),
                _innerDependencies);

            _dateModel.DayProgress
                .Subscribe(_windowModel.DayProgress.Set)
                .AddTo(_disposables);
            _dateModel.DayChanged
                .Subscribe(_ => _windowModel.Date.Set($"{_dateModel.Day} of {_dateModel.MonthName()}, {_dateModel.Year}"))
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _windowsProvider.Recycle(_windowModel);
            _disposables.Dispose();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using CityBuilder.Dependencies;
using Cysharp.Threading.Tasks;
using GameSystems.Common.WindowSystem;
using GameSystems.Implementation.BattleSystem;
using GameSystems.Implementation.GameTime;
using UniRx;
using Utilities.Extensions;
using VContainer.Unity;
using ViewSystem;

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

            _dateModel.DayProgress.Subscribe(_windowModel.DayProgress.Set).AddTo(_disposables);
        }

        public void Dispose()
        {
            _windowsProvider.Recycle(_windowModel);
            _disposables.Dispose();
        }
    }
}
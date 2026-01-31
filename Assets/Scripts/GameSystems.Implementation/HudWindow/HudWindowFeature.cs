using System;
using System.Threading.Tasks;
using CityBuilder.Dependencies;
using Cysharp.Threading.Tasks;
using GameSystems.Common.WindowSystem;
using GameSystems.Implementation.BattleSystem;
using VContainer.Unity;
using ViewSystem;

namespace CityBuilder.GameSystems.Implementation.HudWindow
{
    public class HudWindowFeature : IInitializable, IDisposable
    {
        private readonly IWindowsProvider _windowsProvider;

        private readonly IDependencyContainer _innerDependencies = new DependencyContainer();
        
        private HudWindowModel _windowModel;

        public HudWindowFeature(IWindowsProvider windowsProvider)
        {
            _windowsProvider = windowsProvider;
            
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
        }

        public void Dispose()
        {
            _windowsProvider.Recycle(_windowModel);
        }
    }
}
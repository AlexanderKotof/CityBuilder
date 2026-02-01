using System.Collections.Generic;
using System.Threading.Tasks;
using CityBuilder.Dependencies;
using GameSystems.Common.ViewSystem.ViewsProvider;
using GameSystems.Common.WindowSystem.Window;
using UniRx;
using UnityEngine;
using Utilities.Extensions;

namespace GameSystems.Common.WindowSystem
{
    public interface IWindowsProvider
    {
        Task<TWindowViewModel> CreateWindow<TWindowViewModel>(
            WindowCreationData creationData,
            IDependencyContainer dependencies)
            where TWindowViewModel : IWindowViewModel, new();

        void Recycle<TWindowViewModel>(TWindowViewModel viewModel) where TWindowViewModel : IWindowViewModel;
    }

    public class WindowsProvider : IWindowsProvider
    {
        private readonly IViewWithModelProvider _viewsProvider;
    
        private readonly Dictionary<IWindowViewModel, IWindowView> _windowViews = new ();
    
        private readonly List<IWindowViewModel> _activeViews = new();

        private readonly Transform _windowsParent;
    
        public WindowsProvider(IViewWithModelProvider viewsProvider)
        {
            _viewsProvider = viewsProvider;
            _windowsParent = new GameObject("Windows").transform;
        }

        public async Task<TWindowViewModel> CreateWindow<TWindowViewModel>(
            WindowCreationData creationData,
            IDependencyContainer dependencies)
            where TWindowViewModel : IWindowViewModel, new()
        {
            var assetKey = creationData.AssetKey;
            var viewModel = new TWindowViewModel();
            var view = await _viewsProvider.ProvideViewWithModel<TWindowViewModel, WindowViewBase<TWindowViewModel>>(assetKey, viewModel, dependencies, _windowsParent);
        
            viewModel.IsActive.Subscribe(OnViewActivityChanged);
            viewModel.Close.Subscribe(Close);
        
            _windowViews.Add(viewModel, view);
        
            return viewModel;
        
            void OnViewActivityChanged(bool isActive)
            {
                switch (_activeViews.Contains(viewModel))
                {
                    case true when isActive == false:
                        _activeViews.Remove(viewModel);
                        break;
                    case false when isActive:
                        _activeViews.Add(viewModel);
                        break;
                }
            
                PostProcessWindowsActivity();
            }
        
            void Close(Unit _) => viewModel.IsActive.Set(false);
        }
    
        private void PostProcessWindowsActivity()
        {
            //TODO: postprocess windows
        }

        public void Recycle<TWindowViewModel>(TWindowViewModel viewModel) where TWindowViewModel : IWindowViewModel
        {
            if (_windowViews.Remove(viewModel))
            {
                viewModel.Dispose();
                _viewsProvider.Recycle(viewModel);
            }
        }
    }
}
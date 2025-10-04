using System.Collections.Generic;
using System.Threading.Tasks;
using CityBuilder.Dependencies;
using UnityEngine;
using ViewSystem;

public class WindowsProvider
{
    private readonly IViewWithModelProvider _viewsProvider;
    
    private readonly Dictionary<IWindowViewModel, IWindowView> _views = new ();
    
    private readonly List<IWindowViewModel> _activeViews = new();

    private Transform _windowsParent;
    
    public WindowsProvider(IViewWithModelProvider viewsProvider)
    {
        _viewsProvider = viewsProvider;
    }

    public async Task<TWindowViewModel> CreateWindow<TWindowViewModel>(
        WindowCreationData creationData,
        IDependencyContainer dependencies)
        where TWindowViewModel : IWindowViewModel, new()
    {
        var assetKey = creationData.AssetKey;
        var viewModel = new TWindowViewModel();
        var view = await _viewsProvider.ProvideViewWithModel<TWindowViewModel, WindowViewBase<TWindowViewModel>>(assetKey, viewModel, dependencies, _windowsParent);
        
        viewModel.IsActive.AddListener(OnViewActivityChanged);
        viewModel.Close.AddListener(Close);
        
        _views.Add(viewModel, view);
        
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
        
        void Close() => viewModel.IsActive.Set(false);
    }
    
    private void PostProcessWindowsActivity()
    {
        //TODO: postprocess windows
    }

    public void Recycle<TWindowViewModel>(TWindowViewModel viewModel) where TWindowViewModel : IWindowViewModel
    {
        if (_views.Remove(viewModel))
        {
            viewModel.IsActive.Dispose();
            viewModel.Close.Dispose();
            _viewsProvider.Recycle(viewModel);
        }
    }
}
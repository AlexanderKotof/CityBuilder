using System;
using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using Configs;
using Cysharp.Threading.Tasks;
using GameSystems.Implementation.GameInteractionFeature;
using JetBrains.Annotations;
using VContainer.Unity;
using Views.Implementation.BuildingSystem;
using ViewSystem;

namespace GameSystems.Implementation.BuildingsFeature
{
    public class BuildingsViewFeature : IInitializable, IDisposable
    {
        private readonly BuildingViewCollection _buildingViewsController;
        private readonly WindowsProvider _windowsProvider;
        private readonly IDependencyContainer _container;
        private readonly InteractionModel _interactionModel;
        private readonly BuildingManager _buildingManager;
        
        private BuildingInfoWindowModel _buildingWindowViewModel;
        public BuildingsViewFeature(BuildingManager manager, BuildingsModel model, InteractionModel interactionModel, IViewWithModelProvider viewWithModelProvider, WindowsProvider windowsProvider)
        {
            _container = new DependencyContainer();
            _container.Register(viewWithModelProvider);
            _buildingViewsController = new BuildingViewCollection(model, _container);
            
            _interactionModel = interactionModel;
            _buildingManager = manager;
            _windowsProvider = windowsProvider;
        }
        
        public void Initialize()
        {
            Init().Forget();
        }
        
        public void Dispose()
        {
            _interactionModel.SelectedCell.Unsubscribe(OnSelectedCellUpdated);
            
            _windowsProvider.Recycle(_buildingWindowViewModel);
            _buildingViewsController.Deinit();
        }
        
        private async UniTask Init()
        {
            _buildingViewsController.Initialize();

            _buildingWindowViewModel = await _windowsProvider.CreateWindow<BuildingInfoWindowModel>(
                new WindowCreationData("BuildingWindow", 0),
                _container);
            
            _interactionModel.SelectedCell.Subscribe(OnSelectedCellUpdated);
        }
        
        private void OnSelectedCellUpdated([CanBeNull] CellModel cellModel)
        {
            if (cellModel != null && _buildingManager.TryGetBuilding(cellModel, out var building))
            {
                _buildingWindowViewModel.SelectedBuilding.Set(building);
                return;
            }
            
            _buildingWindowViewModel.SelectedBuilding.Set(null);
        }
    }
}
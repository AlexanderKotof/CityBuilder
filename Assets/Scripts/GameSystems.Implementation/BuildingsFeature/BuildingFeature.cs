using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using Configs;
using GameSystems.Implementation.GameInteractionFeature;
using JetBrains.Annotations;
using Views.Implementation.BuildingSystem;
using ViewSystem;

namespace GameSystems.Implementation.BuildingsFeature
{
    public class BuildingFeature : GameSystemBase
    {
        private readonly BuildingViewCollection _buildingViewsController;
        private readonly WindowsProvider _windowsProvider;
        private readonly IDependencyContainer _container;
        private BuildingInfoWindowModel _buildingWindowViewModel;
        private readonly InteractionModel _interactionModel;

        public BuildingManager BuildingManager { get; private set; }
        public BuildingsModel Model => BuildingManager.Model;
        
        public BuildingFeature(IDependencyContainer container) : base(container)
        {
            var configProvider = container.Resolve<GameConfigProvider>();
            var gridManager = container.Resolve<GridManager>();
            _interactionModel = container.Resolve<InteractionModel>();
            _container = container;
            
            BuildingManager = new BuildingManager(
                configProvider,
                gridManager);

            _buildingViewsController = new (Model, container);

            _windowsProvider = container.Resolve<WindowsProvider>();
        }

        public override async Task Init()
        {
            BuildingManager.Init();
            _buildingViewsController.Initialize();

            _buildingWindowViewModel = await _windowsProvider.CreateWindow<BuildingInfoWindowModel>(
                new WindowCreationData("BuildingWindow", 0),
                _container);
            
            _interactionModel.SelectedCell.Subscribe(OnSelectedCellUpdated);
        }

        public override Task Deinit()
        {
            _interactionModel.SelectedCell.Unsubscribe(OnSelectedCellUpdated);
            
            _windowsProvider.Recycle(_buildingWindowViewModel);
            _buildingViewsController.Deinit();
            return Task.CompletedTask;
        }
        
        private void OnSelectedCellUpdated([CanBeNull] CellModel cellModel)
        {
            if (cellModel != null && BuildingManager.TryGetBuilding(cellModel, out var building))
            {
                _buildingWindowViewModel.SelectedBuilding.Set(building);
                return;
            }
            
            _buildingWindowViewModel.SelectedBuilding.Set(null);
        }
    }
}
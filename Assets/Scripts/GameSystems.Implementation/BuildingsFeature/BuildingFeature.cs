using CityBuilder.Dependencies;
using CityBuilder.Grid;
using GameSystems;
using ViewSystem;
using ViewSystem.Implementation;

namespace CityBuilder.BuildingSystem
{
    public class BuildingFeature : GameSystemBase
    {
        private readonly ViewWithModelProvider _viewWithModelProvider;
        private readonly BuildingViewCollection _buildingViewsController;
        
        public BuildingManager BuildingManager { get; private set; }
        public BuildingsModel Model => BuildingManager.Model;
        
        public BuildingFeature(IDependencyContainer container) : base(container)
        {
            var gameConfiguration = container.Resolve<GameConfigurationSo>();
            var gridManager = container.Resolve<GridManager>();
            BuildingManager = new BuildingManager(
                gameConfiguration.BuildingsConfig,
                gridManager);
            
            var viewsProvider = container.Resolve<IViewsProvider>();
            
            _viewWithModelProvider = new ViewWithModelProvider(viewsProvider, container);

            _buildingViewsController = new (Model, _viewWithModelProvider);
        }

        public override void Init()
        {
            BuildingManager.Init();
            _buildingViewsController.Initialize();
        }

        public override void Deinit()
        {
            _buildingViewsController.Deinit();
            _viewWithModelProvider.Deinit();
        }
    }
}
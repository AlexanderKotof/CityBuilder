using System.Threading.Tasks;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using Configs;
using GameSystems;
using Views.Implementation.BuildingSystem;
using ViewSystem;

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
            var configProvider = container.Resolve<GameConfigProvider>();
            var gridManager = container.Resolve<GridManager>();
            
            BuildingManager = new BuildingManager(
                configProvider,
                gridManager);
            
            var viewsProvider = container.Resolve<IViewsProvider>();
            
            _viewWithModelProvider = new ViewWithModelProvider(viewsProvider, container);

            _buildingViewsController = new (Model, _viewWithModelProvider);
        }

        public override Task Init()
        {
            BuildingManager.Init();
            _buildingViewsController.Initialize();
            return Task.CompletedTask;
        }

        public override Task Deinit()
        {
            _buildingViewsController.Deinit();
            _viewWithModelProvider.Deinit();
            return Task.CompletedTask;
        }
    }
}
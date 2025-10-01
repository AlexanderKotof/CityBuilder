using CityBuilder.Dependencies;
using GameSystems;
using ViewSystem;
using ViewSystem.Implementation;

namespace CityBuilder.BuildingSystem
{
    public class BuildingViewsFeature : GameSystemBase
    {
        private readonly IViewsProvider _viewsProvider;
        private readonly ViewWithModelProvider _viewWithModelProvider;
        private readonly BuildingViewCollection _buildingViewsController;

        public BuildingViewsFeature(IDependencyContainer container) : base(container)
        {
            var buildingsModel = container.Resolve<BuildingsModel>();
            var viewsProvider = container.Resolve<IViewsProvider>();
            
            _viewWithModelProvider = new ViewWithModelProvider(viewsProvider, container);

            _buildingViewsController = new (buildingsModel, _viewWithModelProvider);
        }
        
        public override void Init()
        {
            _buildingViewsController.Initialize();
        }

        public override void Deinit()
        {
            _buildingViewsController.Deinit();
            _viewWithModelProvider.Deinit();
        }
    }
}
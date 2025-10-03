using CityBuilder.BuildingSystem;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BuildingSystem
{
    public class BuildingViewCollection : ReactiveCollectionViewBase<BuildingModel, BuildingView>
    {
        public BuildingViewCollection(
            BuildingsModel model,
            IViewWithModelProvider viewsProvider) : base(model.Buildings, viewsProvider, new GameObject("---Buildings Root---").transform)
        {
        }

        protected override string ProvideAssetKey(BuildingModel viewModel)
        {
            return viewModel.Config.AssetKey;
        }
    }
}
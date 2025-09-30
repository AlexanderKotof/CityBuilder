using CityBuilder.BuildingSystem;
using UnityEngine;

namespace ViewSystem.Implementation
{
    public class BuildingViewCollection : ReactiveCollectionViewBase<BuildingModel, BuildingView>
    {
        public BuildingViewCollection(
            BuildingsModel model,
            WindowViewsProvider viewsProvider) : base(model.Buildings, viewsProvider, new GameObject("---Buildings Root---").transform)
        {
        }

        protected override string ProvideAssetKey(BuildingModel viewModel)
        {
            return viewModel.Config.AssetKey;
        }
    }
}
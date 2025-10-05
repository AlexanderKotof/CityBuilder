using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BuildingSystem
{
    public class BuildingViewCollection : ReactiveCollectionViewBase<BuildingModel, BuildingView>
    {
        public BuildingViewCollection(
            BuildingsModel model,
            IDependencyContainer dependencies) : base(model.Buildings, dependencies, new GameObject("---Buildings Root---").transform)
        {
        }

        protected override void OnViewAdded(BuildingModel viewModel, BuildingView view)
        {
            base.OnViewAdded(viewModel, view);
            viewModel.ThisTransform.Set(view.transform);
        }

        protected override string ProvideAssetKey(BuildingModel viewModel)
        {
            return viewModel.Config.AssetKey;
        }
    }
}
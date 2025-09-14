using CityBuilder.BuildingSystem;
using UnityEngine;

namespace ViewSystem.Implementation
{
    public class BuildingViewCollection : ReactiveCollectionViewBase<BuildingModel, BuildingView>
    {
        private static Transform Root => new GameObject("---Buildings Root---").transform;
        public BuildingViewCollection(
            BuildingsModel model,
            ViewsProvider viewsProvider) : base(model.Buildings, viewsProvider, Root)
        {
        }

        protected override GameObject ProvideAsset(BuildingModel viewModel)
        {
            return viewModel.Config.Prefab;
        }
    }
}
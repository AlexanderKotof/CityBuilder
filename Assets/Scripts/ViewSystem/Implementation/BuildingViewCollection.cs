using CityBuilder.BuildingSystem;
using UnityEngine;

namespace ViewSystem.Implementation
{
    public class BuildingViewCollection : ReactiveCollectionViewBase<Building, BuildingView>
    {
        private static Transform Root => new GameObject("---Buildings Root---").transform;
        public BuildingViewCollection(
            BuildingsModel model,
            ViewsProvider viewsProvider) : base(model.Buildings, viewsProvider, Root)
        {
        }

        protected override GameObject ProvideAsset(Building viewModel)
        {
            return viewModel.Config.Prefab;
        }
    }
}
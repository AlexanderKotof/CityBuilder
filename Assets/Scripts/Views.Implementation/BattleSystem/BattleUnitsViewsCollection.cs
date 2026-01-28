using CityBuilder.Dependencies;
using CityBuilder.Reactive;
using GameSystems.Implementation.BattleSystem;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BattleSystem
{
    public class BattleUnitsViewsCollection : ReactiveCollectionViewBase<BattleUnitBase, BattleUnitBaseView>
    {
        public BattleUnitsViewsCollection(
            ReactiveCollection<BattleUnitBase> collection,
            IDependencyContainer dependencies,
            Transform parent) : base(collection, dependencies, parent)
        {
        }

        protected override void OnViewAdded(BattleUnitBase viewModel, BattleUnitBaseView view)
        {
            base.OnViewAdded(viewModel, view);
            
            var transform = view.ThisTransform;
            viewModel.ThisTransform.Set(transform);
            transform.position = viewModel.StartPosition.Value;
        }

        protected override string ProvideAssetKey(BattleUnitBase viewModel)
        {
            return viewModel.Config.AssetKey;
        }
    }
}
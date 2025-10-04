using CityBuilder.Dependencies;
using CityBuilder.Reactive;
using GameSystems.Implementation.BattleSystem;
using UnityEngine;
using ViewSystem;

namespace Views.Implementation.BattleSystem
{
    public class BattleUnitsViewsCollection : ReactiveCollectionViewBase<BattleUnitModel, BattleUnitBaseView>
    {
        public BattleUnitsViewsCollection(
            ReactiveCollection<BattleUnitModel> collection,
            IDependencyContainer dependencies,
            Transform parent) : base(collection, dependencies, parent)
        {
        }

        protected override string ProvideAssetKey(BattleUnitModel viewModel)
        {
            return viewModel.Config.AssetKey;
        }
    }
}
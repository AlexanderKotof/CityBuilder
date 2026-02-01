using System;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem;
using GameSystems.Common.ViewSystem.ViewsProvider;
using GameSystems.Common.WindowSystem;
using GameSystems.Implementation.BuildingSystem.Domain;
using GameSystems.Implementation.GameInteractionFeature;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Utilities.Extensions;
using VContainer.Unity;
using Views.Implementation.BuildingSystem;
using ViewSystem;

namespace GameSystems.Implementation.BuildingSystem.Features
{
    public class BuildingsViewFeature : IInitializable, IDisposable
    {
        private readonly ViewsCollectionController<BuildingView> _buildingViewsController;
        private readonly IWindowsProvider _windowsProvider;
        private readonly IDependencyContainer _container;
        private readonly InteractionModel _interactionModel;
        private readonly BuildingManager _buildingManager;
        private readonly BuildingsModel _model;

        private BuildingInfoWindowModel _buildingWindowViewModel;
        private readonly CompositeDisposable _subscriptions = new();
        private BuildingView _selectedView;

        public BuildingsViewFeature(BuildingManager manager, BuildingsModel model, InteractionModel interactionModel, IViewsProvider viewsProvider, IWindowsProvider windowsProvider)
        {
            _interactionModel = interactionModel;
            _buildingManager = manager;
            _model = model;
            _windowsProvider = windowsProvider;
            var root = new GameObject("---Buildings Root---").transform;
            _buildingViewsController = new ViewsCollectionController<BuildingView>(viewsProvider, defaultParent: root);
        }
        
        public void Initialize()
        {
            SubscribeOnBuildings();
            InitWindow().Forget();
        }

        private void SubscribeOnBuildings()
        {
            _model.Buildings
                .SubscribeToCollection(b => OnBuildingAdded(b).Forget(), OnBuildingRemoved)
                .AddTo(_subscriptions);

            async UniTaskVoid OnBuildingAdded(BuildingModel obj)
            {
                var view = await _buildingViewsController.AddView(obj.Config.AssetKey, obj);
                view.Initialize(obj);
                obj.ThisTransform.Value = view.transform;
            }
            void OnBuildingRemoved(BuildingModel obj)
            {
                _buildingViewsController.Return(obj);
            }
        }
        
        public void Dispose()
        {
            _windowsProvider.Recycle(_buildingWindowViewModel);
            
            _buildingViewsController.Dispose();
            _subscriptions.Dispose();
        }

        public BuildingView GetBuildingView(BuildingModel buildingModel)
        {
            return _buildingViewsController.GetView(buildingModel);
        }
        
        private async UniTask InitWindow()
        {
            _buildingWindowViewModel = await _windowsProvider.CreateWindow<BuildingInfoWindowModel>(
                new WindowCreationData("BuildingWindow", 0),
                _container);
            
            _interactionModel.SelectedCell.Subscribe(OnSelectedCellUpdated).AddTo(_subscriptions);
        }
        
        private void OnSelectedCellUpdated([CanBeNull] CellModel cellModel)
        {
            if (_selectedView != null)
            {
                _selectedView.SetUiActive(false);
            }
            
            if (cellModel != null && _buildingManager.TryGetBuilding(cellModel, out var building))
            {
                SelectBuilding(building);
                return;
            }
            
            _buildingWindowViewModel.SelectedBuilding.Set(null);
        }

        private void SelectBuilding(BuildingModel building)
        {
            // Building window selection update
            _buildingWindowViewModel.SelectedBuilding.Set(building);
            
            if (_buildingViewsController.TryGetView(building, out var view))
            {
                _selectedView = view;
                _selectedView.SetUiActive(true);
            }
        }
    }
}
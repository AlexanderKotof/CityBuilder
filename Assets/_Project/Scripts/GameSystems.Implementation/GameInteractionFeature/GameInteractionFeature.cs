using System;
using CityBuilder.Configs.Scriptable;
using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.GameSystems.Implementation.BuildingSystem;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Features;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine;
using CityBuilder.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature
{
    public class GameInteractionFeature : IInitializable, IDisposable
    {
        private readonly BuildingManager _buildingManager;
        private readonly MergeBuildingsFeature _mergeBuildingsFeature;
        private readonly InteractionModel _interactionModel;
        private readonly DraggingContentController _draggingContentController;
        private PlayerInteractionStateMachine? _playerInteractionStateMachine;
    
        private readonly IViewsProvider _viewsProvider;
        private readonly CommonGameSettingsSo _settings;
        private readonly CompositeDisposable _subscriptions = new();

        public GameInteractionFeature(BuildingManager buildingManager, MergeBuildingsFeature mergeBuildingsFeature, InteractionModel interactionModel, DraggingContentController draggingContentController)
        {
            _buildingManager = buildingManager;
            _mergeBuildingsFeature = mergeBuildingsFeature;
            _interactionModel = interactionModel;
            _draggingContentController = draggingContentController;
        }

        public void Dispose()
        {
            //_subscriptions.Dispose();
        }

        public void Initialize()
        {
            //_interactionModel.DragAndDropped.Subscribe(e => TryDropContent(e.from, e.to)).AddTo(_subscriptions);
        }

        // private void TryDropContent(CellModel fromCell, CellModel toCellModel)
        // {
        //     if (!_buildingManager.TryGetBuilding(fromCell, out var fromBuilding))
        //     {
        //         _draggingContentController.CancelDrag();
        //         return;
        //     }
        //
        //     if (_buildingManager.TryMoveBuilding(toCellModel, fromBuilding))
        //     {
        //         _interactionModel.SelectedCell.Set(toCellModel);
        //         return;
        //     }
        //     
        //     if (!_buildingManager.TryGetBuilding(toCellModel, out var toBuilding))
        //     {
        //         _draggingContentController.CancelDrag();
        //         return;
        //     }
        //
        //     if (_mergeBuildingsFeature.TryMergeBuildingsFromTo(fromBuilding, toBuilding))
        //     {
        //         _interactionModel.SelectedCell.Set(toCellModel);
        //     }
        //     else
        //     {
        //         _draggingContentController.CancelDrag();
        //     }
        // }
    }
}
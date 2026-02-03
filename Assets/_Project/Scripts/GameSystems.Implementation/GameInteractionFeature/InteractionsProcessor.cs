using System;
using System.Linq;
using CityBuilder.GameSystems.Implementation.BuildingSystem;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Features;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.Utilities.Extensions;
using JetBrains.Annotations;
using UniRx;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature
{
    /// <summary>
    /// Процессит интеракции игрока с игрой, так же отвечает за выбор текущего действия
    /// </summary>
    public class InteractionsProcessor : IInitializable, IDisposable
    {
        private readonly InteractionModel _interactionModel;
        private readonly BuildingManager _buildingManager;
        private readonly MergeBuildingsFeature _mergeBuildingsFeature;
        
        private readonly CompositeDisposable _subscriptions = new();
        
        public InteractionsProcessor(InteractionModel interactionModel, BuildingManager buildingManager, MergeBuildingsFeature mergeBuildingsFeature)
        {
            _interactionModel = interactionModel;
            _buildingManager = buildingManager;
            _mergeBuildingsFeature = mergeBuildingsFeature;
        }
        
        public void Initialize()
        {
            _interactionModel.DragAndDropped.Subscribe(e => ExecuteSelected()).AddTo(_subscriptions);
            _interactionModel.HoveredCell.Subscribe(OnHoveredCellChanged).AddTo(_subscriptions);
            _interactionModel.ExecuteAction.Subscribe(ProcessExecuteAction).AddTo(_subscriptions);
        }
        
        public void Dispose()
        {
            _subscriptions.Dispose();
        }
        
        private void ExecuteSelected() => _interactionModel.ExecuteCurrent();

        private void ProcessExecuteAction(IPlayerAction action)
        {
            var processor = GetActionProcessor(action);
            processor();
        }

        private Action GetActionProcessor(IPlayerAction action)
        {
            return action switch
            {
                MoveBuildingAction moveBuildingAction => () => ProcessMoveBuildingAction(moveBuildingAction),
                MergeWithRecipeBuildingsAction mergeRecipeAction => () => ProcessMergeRecipe(mergeRecipeAction),
                MergeLevelUoBuildingsAction mergeLevelUp => () => ProcessLevelUp(mergeLevelUp),
                RejectedAction rejectedAction => () => ProcessActionRejected(rejectedAction),
                _ => throw new NotImplementedException(),
            };
        }
        
        private void OnHoveredCellChanged([CanBeNull] CellModel cell)
        {
            var draggedCell = _interactionModel.DraggedCell.Value;
            if (cell == null || draggedCell == null)
            {
                return;
            }
            
            //TODO: switch logic?
            
            //TODO: logic may change depend on dragged content
            
            if (!_buildingManager.TryGetBuilding(draggedCell, out var fromBuilding))
                return;

            if (_buildingManager.CanPlaceBuilding(cell, fromBuilding))
            {
                _interactionModel.SelectAction(new MoveBuildingAction(fromBuilding, cell));
                return;
            }

            if (!_buildingManager.TryGetBuilding(cell, out var toBuilding))
            {
                _interactionModel.SelectAction(new RejectedAction(_interactionModel.HoveredCell.Value));
                return;
            }

            if (_mergeBuildingsFeature.CanLevelUpMerge(toBuilding, fromBuilding, out var buildings))
            {
                _interactionModel.SelectAction(new MergeLevelUoBuildingsAction(fromBuilding, toBuilding, buildings.ToList()));
                return;
            }
            
            if (_mergeBuildingsFeature.CanRecipeMerge(toBuilding, fromBuilding, out var recipe, out buildings))
            {
                _interactionModel.SelectAction(new MergeWithRecipeBuildingsAction(fromBuilding, toBuilding, recipe, buildings.ToList()));
                return;
            }
            
            _interactionModel.SelectAction(new RejectedAction(_interactionModel.HoveredCell.Value));
        }

        private void ProcessActionRejected(RejectedAction action)
        {
            _interactionModel.CancelDrag.Execute();
            
            //TODO: some animation?
        }

        private void ProcessLevelUp(MergeLevelUoBuildingsAction action)
        {
            _mergeBuildingsFeature.MergeUpgrade(
                action.DraggedBuilding,
                action.ToBuilding,
                action.InvolvedBuildings);
            _interactionModel.SelectedCell.Set(action.ToBuilding.OccupiedCells.First());
        }

        private void ProcessMergeRecipe(MergeWithRecipeBuildingsAction action)
        {
            _mergeBuildingsFeature.MergeWithRecipe(
                action.DraggedBuilding,
                action.ToBuilding,
                action.Recipe,
                action.InvolvedBuildings);
            _interactionModel.SelectedCell.Set(action.ToBuilding.OccupiedCells.First());
        }

        private void ProcessMoveBuildingAction(MoveBuildingAction action)
        {
            _buildingManager.TryMoveBuilding(action.ToCell, action.Building);
            _interactionModel.SelectedCell.Set(action.ToCell);
        }
    }
}
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using UnityEngine;

namespace InteractionStateMachine
{
    public class EmptyInteractionState : InteractionState
    {
        private readonly BuildingManager _buildingManager;

        public EmptyInteractionState(DependencyContainer dependencyContainer) : base(dependencyContainer)
        {
            _buildingManager = dependencyContainer.Resolve<BuildingManager>();
        }

        protected override void OnEnterState()
        {
            base.OnEnterState();
        }

        protected override void OnExitState()
        {
            base.OnExitState();
        }

        protected override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && 
                Raycaster.TryGetCellFromScreenPoint(Input.mousePosition, out var cell))
            {
                _buildingManager.TryPlaceDefaultBuilding(cell);
            }
        }

        protected override void ProcessClick(CellModel cellModel)
        {
            TrySelectCell(cellModel);
        }

        protected override void OnCellSelected()
        {
            if (InteractionModel.SelectedCell == null)
            {
                return;
            }
            
            ChangeState<CellSelectedInteractionState>();
        }

        protected override void ProcessDragStarted(CellModel cellModel)
        {
            base.ProcessDragStarted(cellModel);
            StartDragCell(cellModel);
        }
    }
}
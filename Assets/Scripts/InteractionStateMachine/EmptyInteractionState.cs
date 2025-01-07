using BuildingSystem;
using UnityEngine;

namespace InteractionStateMachine
{
    public class EmptyInteractionState : InteractionState
    {
        private readonly BuildingManager _buildingManager;

        public EmptyInteractionState(Dependencies dependencies) : base(dependencies)
        {
            _buildingManager = dependencies.Resolve<BuildingManager>();
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
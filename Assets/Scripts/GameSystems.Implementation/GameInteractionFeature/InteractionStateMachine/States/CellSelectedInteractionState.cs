using CityBuilder.Dependencies;
using CityBuilder.Grid;

namespace GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States
{
    public class CellSelectedInteractionState : InteractionState
    {
        public CellSelectedInteractionState(IDependencyContainer dependencyContainer) : base(dependencyContainer)
        {
        }

        protected override void OnEnterState()
        {
            base.OnEnterState();
            //LightenCell(InteractionModel.SelectedCell.Value);
        }

        protected override void OnExitState()
        {
            base.OnExitState();
           // LightenCell(null);
        }
        
        protected override void ProcessClick(CellModel cellModel)
        {
            TrySelectCell(cellModel);
        }

        protected override void OnCellSelected()
        {
            if (InteractionModel.SelectedCell == null)
            {
                ChangeState<EmptyInteractionState>();
            }
        }

        protected override void ProcessDragStarted(CellModel cellModel)
        {
            base.ProcessDragStarted(cellModel);
            StartDragCell(cellModel);
        }

        protected override void ProcessRightClick(CellModel cellModel)
        {
            base.ProcessRightClick(cellModel);
            DeselectCell();
        }

        protected override void TryCancel()
        {
            DeselectCell();
        }
    }
}
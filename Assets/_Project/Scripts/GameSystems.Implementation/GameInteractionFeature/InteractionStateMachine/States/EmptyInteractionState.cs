using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States
{
    public class EmptyInteractionState : InteractionState
    {
        protected override void OnEnterState()
        {
            base.OnEnterState();
        }

        protected override void OnExitState()
        {
            base.OnExitState();
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
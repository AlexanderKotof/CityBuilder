using CityBuilder.Dependencies;
using CityBuilder.Grid;

namespace GameSystems.Implementation.GameInteraction.InteractionStateMachine.States
{
    public class CellSelectedInteractionState : InteractionState
    {
        public CellSelectedInteractionState(IDependencyContainer dependencyContainer) : base(dependencyContainer)
        {
        }

        protected override void OnEnterState()
        {
            base.OnEnterState();
            //ToDo: show selected cell info
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
using CityBuilder.Dependencies;
using CityBuilder.Grid;

namespace GameSystems.Implementation.GameInteraction.InteractionStateMachine.States
{
    public class EmptyInteractionState : InteractionState
    {
        public EmptyInteractionState(IDependencyContainer dependencyContainer) : base(dependencyContainer)
        {
        }

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
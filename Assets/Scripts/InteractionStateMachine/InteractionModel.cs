using CityBuilder.Grid;

namespace InteractionStateMachine
{
    public class InteractionModel
    {
        public CellModel? SelectedCell { get;  set; }
        
        public CellModel? DraggedCell { get;  set; }
    }
}
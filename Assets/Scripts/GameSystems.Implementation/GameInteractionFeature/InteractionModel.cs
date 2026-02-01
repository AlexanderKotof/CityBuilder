using CityBuilder.Grid;
using UniRx;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class InteractionModel
    {
        public readonly ReactiveProperty<CellModel> HoveredCell = new(null);
        public readonly ReactiveProperty<CellModel?> SelectedCell = new(null);
        public readonly ReactiveProperty<CellModel?> DraggedCell = new(null);
    }
}
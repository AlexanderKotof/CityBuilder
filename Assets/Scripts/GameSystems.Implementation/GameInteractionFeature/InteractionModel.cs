using CityBuilder.Grid;
using CityBuilder.Reactive;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class InteractionModel
    {
        public readonly ReactiveProperty<CellModel?> SelectedCell = new(null);
        public readonly ReactiveProperty<CellModel?> DraggedCell = new(null);
    }
}
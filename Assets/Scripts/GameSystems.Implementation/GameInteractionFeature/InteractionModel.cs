using CityBuilder.Grid;
using CityBuilder.Reactive;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class InteractionModel
    {
        public ReactiveProperty<CellModel?> SelectedCell { get; } = new();

        public ReactiveProperty<CellModel?> DraggedCell { get; } = new();
    }
}
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.Utilities.Extensions;
using UniRx;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature
{
    public class InteractionModel
    {
        public IReadOnlyReactiveProperty<CellModel> HoveredCell => _hoveredCell;
        private readonly ReactiveProperty<CellModel> _hoveredCell = new(null);

        public readonly ReactiveProperty<CellModel?> SelectedCell = new(null);
        public readonly ReactiveProperty<CellModel?> DraggedCell = new(null);
        
        public readonly ReactiveCommand<(CellModel from, CellModel to)> DragAndDropped = new();
        
        public CellModel LastHoveredCell { get; private set; }

        public void SetHovered(CellModel cell)
        {
            _hoveredCell.Set(cell);
            LastHoveredCell = cell;
        }

        public void ClearHover()
        {
            _hoveredCell.Set(null);
        }
    }
}
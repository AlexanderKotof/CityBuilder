using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.Utilities.Extensions;
using UniRx;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature
{
    public class InteractionModel
    {
        public readonly ReactiveProperty<CellModel> HoveredCell = new(null);
        public readonly ReactiveProperty<CellModel?> SelectedCell = new(null);
        public readonly ReactiveProperty<CellModel?> DraggedCell = new(null);
        
        public CellModel LastHoveredCell { get; private set; }

        public void SetHovered(CellModel cell)
        {
            HoveredCell.Set(cell);
            LastHoveredCell = cell;
        }

        public void ClearHover()
        {
            HoveredCell.Set(null);
        }
    }
}
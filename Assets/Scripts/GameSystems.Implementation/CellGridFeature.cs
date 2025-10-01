using CityBuilder.Dependencies;
using CityBuilder.Grid;
using GameSystems;
using UnityEngine;

public class CellGridFeature : GameSystemBase
{
    public GridManager GridManager { get; private set; }

    public CellGridFeature(IDependencyContainer container) : base(container)
    {
        GridManager = new GridManager();
    }

    public override void Init()
    {
        RegisterGrids();
    }

    public override void Deinit() { }
    
    private void RegisterGrids()
    {
        var grids = GameObject.FindObjectsOfType<GridComponent>();
        foreach (var grid in grids)
        {
            GridManager.RegisterGrid(grid);
        }
    }
}
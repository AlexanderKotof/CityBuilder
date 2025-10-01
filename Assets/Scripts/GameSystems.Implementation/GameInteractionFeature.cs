using System;
using System.Threading.Tasks;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using GameSystems;
using InteractionStateMachine;
using UnityEngine;
using ViewSystem;

public class GameInteractionFeature : GameSystemBase, IUpdateGamSystem
{
    public CursorController CursorController { get; private set; }
    public Raycaster Raycaster { get; }
    public DraggingContentController DraggingContentController { get; }

    public InteractionModel InteractionModel { get; } = new InteractionModel();
    
    private PlayerInteractionStateMachine? _playerInteractionStateMachine;
    
    private readonly IViewsProvider _viewsProvider;
    private readonly GameConfigurationSo _settings;
    private Transform _cursor;

    public GameInteractionFeature(IDependencyContainer container) : base(container)
    {
        _settings = container.Resolve<GameConfigurationSo>();
        
        var raycasterCamera = container.Resolve<Camera>();
        var layerMask = _settings.InteractionRaycastLayerMask;
        var gridManager = container.Resolve<GridManager>();
        
        _viewsProvider = container.Resolve<IViewsProvider>();
        
        Raycaster = new Raycaster(raycasterCamera, layerMask, gridManager);
        
        DraggingContentController = new DraggingContentController();
    }

    public override async Task Init()
    {
        _cursor = await _viewsProvider.ProvideViewAsync<Transform>(_settings.SelectionAssetReferenceKey);
        CursorController = new CursorController(_cursor);
        
        Container.Register(CursorController);
        
        _playerInteractionStateMachine = new PlayerInteractionStateMachine(
            new EmptyInteractionState(Container),
            new CellSelectedInteractionState(Container),
            new DraggingInteractionState(Container)
        );
        
        _playerInteractionStateMachine.Start(typeof(EmptyInteractionState));
    }

    public override Task Deinit()
    {
        _playerInteractionStateMachine?.Stop();
        _viewsProvider.ReturnView(_cursor);
        return base.Deinit();
    }

    public void Update()
    {
        _playerInteractionStateMachine?.Update();
    }
}
using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using Configs;
using Configs.Extensions;
using Configs.Schemes;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using UnityEngine;
using ViewSystem;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class GameInteractionFeature : GameSystemBase, IUpdateGamSystem
    {
        public CursorController CursorController { get; private set; }
        public Raycaster Raycaster { get; private set; }
        public DraggingContentController DraggingContentController { get; }

        public InteractionModel InteractionModel { get; } = new InteractionModel();
    
        private PlayerInteractionStateMachine? _playerInteractionStateMachine;
    
        private readonly IViewsProvider _viewsProvider;
        private readonly CommonGameSettings _settings;
        private Transform _cursor;
        private CellSelectionController _cellSelectionController;

        public GameInteractionFeature(IDependencyContainer container) : base(container)
        {
            _settings = container.Resolve<GameConfigProvider>().CommonGameSettings();
            var raycasterCamera = container.Resolve<Camera>();
            var gridManager = container.Resolve<GridManager>();
            _viewsProvider = container.Resolve<IViewsProvider>();
        
            LayerMask layerMask = (LayerMask)_settings.InteractionRaycastLayerMask;
            Raycaster = new Raycaster(raycasterCamera, layerMask, gridManager);
        
            DraggingContentController = new DraggingContentController();
        }

        public override async Task Init()
        {
            _cursor = await _viewsProvider.ProvideViewAsync<Transform>(_settings.SelectorAssetReferenceKey);
            CursorController = new CursorController(_cursor);
            
            //TODO: ???
            Container.Register(CursorController);

            var buildingManager = Container.Resolve<BuildingManager>();
            _cellSelectionController =
                new CellSelectionController(CursorController, InteractionModel, buildingManager);
            _cellSelectionController.Init();
        
            _playerInteractionStateMachine = new PlayerInteractionStateMachine(
                new EmptyInteractionState(Container),
                new CellSelectedInteractionState(Container),
                new DraggingInteractionState(Container)
            );
        
            _playerInteractionStateMachine.Start(typeof(EmptyInteractionState));
        }

        public override Task Deinit()
        {
            _cellSelectionController.Deinit();
            _playerInteractionStateMachine?.Stop();
            _viewsProvider.ReturnView(_cursor);
            return base.Deinit();
        }

        public void Update()
        {
            _playerInteractionStateMachine?.Update();
        }
    }
}
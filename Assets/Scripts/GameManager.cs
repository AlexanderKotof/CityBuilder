using System;
using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using GameSystems;
using GameSystems.Implementation.PopulationFeature;
using GameTimeSystem;
using InteractionStateMachine;
using JetBrains.Annotations;
using PlayerInput;
using ResourcesSystem;
using UnityEngine;
using ViewSystem;


public class GameManager : MonoBehaviour, IUnityUpdate
{
    public Camera RaycasterCamera;
    public LayerMask LayerMask;
    public Transform Cursor;
    
    public GameConfigurationSo GameConfiguration;

    private PlayerInputManager _playerInputManager;
    private GridManager _gridManager;
    private ViewsProvider _viewsProvider;

    public CursorController CursorController { get; private set; }
    public Raycaster Raycaster { get; private set; }
    public BuildingManager BuildingManager { get; private set; }
    public ResourcesManager ResourcesManager { get; private set; }
    public DraggingContentController DraggingContentController { get; private set; }
    private PlayerInteractionStateMachine? PlayerInteractionStateMachine;

    private InteractionModel _interactionModel = new InteractionModel();
    
    private Action? UpdateHandler;
    private GameSystemsInitialization _gameSystemsInitialization;
    private DependencyContainer _innerDependencies;

    private void Awake()
    {
        _playerInputManager = new PlayerInputManager();
        _viewsProvider = new ViewsProvider();

        _gridManager = new GridManager();
        
        RegisterGrids();

        //ToDo: move to features/systems
        Raycaster = new Raycaster(RaycasterCamera, LayerMask, _gridManager);
        CursorController = new CursorController(Cursor);
        ResourcesManager = new ResourcesManager(GameConfiguration.ResourcesConfig);
        DraggingContentController = new DraggingContentController();

        _innerDependencies = new DependencyContainer();
        
        _innerDependencies.Register(GameConfiguration);
        _innerDependencies.Register(_playerInputManager);
        _innerDependencies.Register<IViewsProvider>(_viewsProvider);
        _innerDependencies.Register(Raycaster);
        _innerDependencies.Register<IUnityUpdate>(this);
        _innerDependencies.Register(_interactionModel);
        _innerDependencies.Register(CursorController);
        _innerDependencies.Register(DraggingContentController);
        _innerDependencies.Register(ResourcesManager.PlayerResourcesStorage);
        
        InitializeGameSystems(_innerDependencies);
        InitializePlayerInteractionStateMachine(_innerDependencies);
        
        WindowTest();
    }
    
    private void OnDestroy()
    {
        _viewsProvider.Dispose();
        PlayerInteractionStateMachine?.Stop();
        _gameSystemsInitialization.Deinit();
    }

    private async void WindowTest()
    {
        var provider = new WindowViewProvider(_viewsProvider, _innerDependencies);

        var viewModel1 = new BuildingModel(1, null);
        var viewModel2 = new BuildingModel(2, null);

        string assetKey = "BuildingWindow";
        await provider.ProvideViewWithModel(assetKey, viewModel1);
        
        await provider.ProvideViewWithModel(assetKey, viewModel2);

        await Task.Delay(5000);
        
        provider.Recycle(viewModel1);
        
        await Task.Delay(1000);
        provider.Recycle(viewModel2);
        
        await Task.Delay(1000);

        await provider.ProvideViewWithModel(assetKey, viewModel1);
        
        provider.Deinit();
    }

    private void RegisterGrids()
    {
        var grids = GameObject.FindObjectsOfType<GridComponent>();
        foreach (var grid in grids)
        {
            _gridManager.RegisterGrid(grid);
        }
    }

    private void InitializePlayerInteractionStateMachine(DependencyContainer dependencies)
    {
        PlayerInteractionStateMachine = new PlayerInteractionStateMachine(
            new EmptyInteractionState(dependencies),
            new CellSelectedInteractionState(dependencies),
            new DraggingInteractionState(dependencies)
        );
        
        PlayerInteractionStateMachine.Start(typeof(EmptyInteractionState));
    }

    private async void InitializeGameSystems(DependencyContainer dependencies)
    {
        _gameSystemsInitialization = new GameSystemsInitialization(dependencies);
        await _gameSystemsInitialization.Init();
    }
    
    private void Update()
    {
        UpdateHandler?.Invoke();

        _gameSystemsInitialization.Update();
        
        _playerInputManager.Update();

        if (PlayerInteractionStateMachine != null)
        {
            PlayerInteractionStateMachine.Update();
        }
    }
    
    public void SubscribeOnUpdate(Action action)
    {
        UpdateHandler += action;
    }

    public void UnsubscribeOnUpdate(Action action)
    {
        UpdateHandler -= action;
    }

    [CanBeNull] private DateModel _dateModel;
    [CanBeNull] private PopulationModel _populationModel;
    private void OnGUI()
    {
        int MaxIndex = 5;
        for (int i = 0; i < MaxIndex; i++)
        {
            var resourceType = (ResourceType)i;
            var amount = ResourcesManager.PlayerResourcesStorage.GetResourceAmount(resourceType);
            GUI.Label(new Rect(20 + 50 * i, 20, 50, 50), new GUIContent($"{resourceType.ToString()}:\n{amount}"));
        }

        _dateModel ??= _innerDependencies.Resolve<DateModel>();
        if (_dateModel != null)
        {
            GUI.Label(new Rect(20, 70, 1000, 50), new GUIContent(_dateModel.ToString()));
        }
        
        _populationModel ??= _innerDependencies.Resolve<PopulationModel>();
        if (_populationModel != null)
        {
            GUI.Label(new Rect(20, 100, 100, 50),
                new GUIContent($"Population: {_populationModel.CurrentPopulation.Value.ToString()} / {_populationModel.AvailableHouseholds.Value.ToString()} houses"));
        }
    }
}
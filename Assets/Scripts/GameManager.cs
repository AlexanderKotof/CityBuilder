using System;
using System.Collections.Generic;
using System.Linq;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using GameSystems;
using InteractionStateMachine;
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
    
    public List<GameObject> windowsPrefabs;

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

    private void Awake()
    {
        _playerInputManager = new PlayerInputManager();
        _gridManager = new GridManager();
        
        _viewsProvider = new ViewsProvider();
        
        RegisterGrids();

        Raycaster = new Raycaster(RaycasterCamera, LayerMask, _gridManager);
        CursorController = new CursorController(Cursor);
        ResourcesManager = new ResourcesManager(GameConfiguration.ResourcesConfig);
        BuildingManager = new(GameConfiguration.BuildingsConfig, _gridManager, _viewsProvider);
        DraggingContentController = new DraggingContentController();
        
        InitializePlayerInteractionStateMachine();
        InitializeGameSystems();
        
        WindowTest();
    }

    private void WindowTest()
    {
        var provider = new WindowsProvider(windowsPrefabs, _viewsProvider);

        provider.ProvideWindowView(new BuildingModel(1, null));
        
        provider.ProvideWindowView(new BuildingModel(1, null));
    }

    private void RegisterGrids()
    {
        var grids = GameObject.FindObjectsOfType<GridComponent>();
        foreach (var grid in grids)
        {
            _gridManager.RegisterGrid(grid);
        }
    }

    private void InitializePlayerInteractionStateMachine()
    {
        var dependencies = new DependencyContainer();
        dependencies.Register(_playerInputManager);
        dependencies.Register(BuildingManager);
        dependencies.Register(Raycaster);
        dependencies.Register<IUnityUpdate>(this);
        dependencies.Register(_interactionModel);
        dependencies.Register(CursorController);
        dependencies.Register(DraggingContentController);

        PlayerInteractionStateMachine = new PlayerInteractionStateMachine(
            new EmptyInteractionState(dependencies),
            new CellSelectedInteractionState(dependencies),
            new DraggingInteractionState(dependencies)
        );
        
        PlayerInteractionStateMachine.Start(typeof(EmptyInteractionState));
    }

    private void InitializeGameSystems()
    {
        var dependencies = new DependencyContainer();
        dependencies.Register(_playerInputManager);
        dependencies.Register(BuildingManager);
        dependencies.Register(BuildingManager.Model);
        dependencies.Register(Raycaster);
        dependencies.Register<IUnityUpdate>(this);
        dependencies.Register(_interactionModel);
        dependencies.Register(CursorController);
        dependencies.Register(DraggingContentController);
        dependencies.Register(ResourcesManager.PlayerResourcesStorage);
        
        _gameSystemsInitialization = new GameSystemsInitialization(dependencies);
        _gameSystemsInitialization.Init();
    }

    private void OnDestroy()
    {
        PlayerInteractionStateMachine?.Stop();
        _gameSystemsInitialization.Deinit();
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

        /*
        if (Input.GetMouseButtonDown(0) && Raycaster.TryGetCellFromScreenPoint(Input.mousePosition, out var cell))
        {
            OnClickCell(cell);
        }

       */
    }
    
    private Action? UpdateHandler;
    private GameSystemsInitialization _gameSystemsInitialization;


    public void SubscribeOnUpdate(Action action)
    {
        UpdateHandler += action;
    }

    public void UnsubscribeOnUpdate(Action action)
    {
        UpdateHandler -= action;
    }

    private void OnGUI()
    {
        int MaxIndex = 5;
        for (int i = 0; i < MaxIndex; i++)
        {
            var resourceType = (ResourceType)i;
            var amount = ResourcesManager.PlayerResourcesStorage.GetResourceAmount(resourceType);
            GUI.Label(new Rect(20 + 50 * i, 20, 50, 50), new GUIContent($"{resourceType.ToString()}: {amount}"));
        }
    }
}

public interface ICoreModelsProvider
{
    public T GetModel<T>() where T : class;
    public T RegisterModel<T>(T instance) where T : class;
}

public class CoreModelsProvider : ICoreModelsProvider
{
    private readonly Dictionary<Type, object> _models = new();
    public T GetModel<T>() where T : class
    {
        return (T)_models.GetValueOrDefault(typeof(T));
    }

    public T RegisterModel<T>(T instance) where T : class
    {
        return _models.TryAdd(instance.GetType(), instance) ? instance : GetModel<T>();
    }
}
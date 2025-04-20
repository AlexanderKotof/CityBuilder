using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
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

        provider.ProvideWindowView(new Building(1, null));
        
        provider.ProvideWindowView(new Building(1, null));
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
        dependencies.Register(new InteractionModel());
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
        dependencies.Register(Raycaster);
        dependencies.Register<IUnityUpdate>(this);
        dependencies.Register(new InteractionModel());
        dependencies.Register(CursorController);
        dependencies.Register(DraggingContentController);
        
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
}

public class GameSystemsInitialization : IGameSystem
{
    private readonly IDependencyContainer _container;
    
    private readonly Type[] _dependencyContainerType = new [] { typeof(IDependencyContainer) };
    private readonly object[] _constructorParameters;

    public HashSet<Type> GameSystemTypes = new()
    {
        typeof(GameTimeSystem.GameTimeSystem),
        typeof(BuildingsProductionGameSystem),
        
    };

    public readonly List<IGameSystem> _gameSystems = new();

    public GameSystemsInitialization(IDependencyContainer container)
    {
        _container = container;
        _constructorParameters  = new [] { _container };
    }

    public void Init()
    {
        CreateGameSystems();

        InitializeGameSystems();
    }

    public void Deinit()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Deinit();
        }
    }

    public void Update()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Update();
        }
    }

    private void CreateGameSystems()
    {
        Debug.Log("Begin initializing Game Systems");

        foreach (var systemType in GameSystemTypes)
        {
            object? system;
            
            ConstructorInfo constructorWithDependencies = systemType.GetConstructor(_dependencyContainerType);

            if (constructorWithDependencies != null)
            { 
                system = constructorWithDependencies.Invoke(_constructorParameters);
                Debug.Log($"System of type {systemType.Name} has constructor with dependencies");
            }
            else
            {
                ConstructorInfo defaultConstructor = systemType.GetConstructor(Type.EmptyTypes);
                system = defaultConstructor?.Invoke(null);
                Debug.Log($"System of type {systemType.Name} has constructor without dependencies");
            }
            
            if (system == null)
            {
                Debug.LogError($"System of type {systemType.Name} does not have a default constructors.");
                continue;
            }
            
            if (system is not IGameSystem gameSystem)
            {
                Debug.LogError($"System of type {systemType.Name} does not implement IGameSystem.");
                continue;
            }
            
            Debug.Log($"Successfully created game system {systemType.Name}.");
            _gameSystems.Add(gameSystem);
            _container.Register(systemType, gameSystem);
        }
    }
    
    private void InitializeGameSystems()
    {
        foreach (var gameSystem in _gameSystems)
        {
            gameSystem.Init();
        }
    }

}

public interface IGameSystem
{
    void Init();
    void Deinit();
    void Update();
}

public class BuildingsProductionGameSystem : IGameSystem
{
    private readonly BuildingManager _buildingsManager;
    private readonly GameTimeSystem.GameTimeSystem _gameTimeSystem;

    public BuildingsProductionGameSystem(IDependencyContainer dependencies)
    {
        _buildingsManager = dependencies.Resolve<BuildingManager>();
        _gameTimeSystem = dependencies.Resolve<GameTimeSystem.GameTimeSystem>();
    }
    
    public void Init()
    {
        _gameTimeSystem.NewDayStarted += OnNewDayStarted;
    }
    
    public void Deinit()
    {
        _gameTimeSystem.NewDayStarted -= OnNewDayStarted;
    }
    
    private void OnNewDayStarted(int day)
    {
        foreach (var building in _buildingsManager.Model.Buildings)
        {
            var produceFunctions =
                building.Config.BuildingFunctions
                    .Where(function => function is ResourceProductionBuildingFunction)
                    .Cast<ResourceProductionBuildingFunction>();

            foreach (var function in produceFunctions)
            {
                Debug.Log($"Building {building.BuildingName + building.Level} produces {function.ProduceResourcesByTick} at day {day}");
            }
        }
    }
    
    public void Update() { }
}
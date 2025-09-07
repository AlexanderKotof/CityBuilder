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
        dependencies.Register(ResourcesManager.PlayerResources);
        
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
            var amount = ResourcesManager.PlayerResources.GetResourceAmount(resourceType);
            GUI.Label(new Rect(20 + 50 * i, 20, 50, 50), new GUIContent($"{resourceType.ToString()}: {amount}"));
        }
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
        typeof(ProducingFeature.ProducingFeature),
        
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

public abstract class GameSystemBase : IGameSystem
{
    protected IDependencyContainer Container { get; }

    protected GameSystemBase(IDependencyContainer container)
    {
        Container = container;
    }
    
    public abstract void Init();
    public abstract void Deinit();
    public abstract void Update();
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
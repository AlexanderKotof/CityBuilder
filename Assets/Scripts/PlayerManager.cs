using System;
using BuildingSystem;
using InteractionStateMachine;
using PlayerInput;
using SityBuilder.Reactive;
using UnityEngine;
using ViewSystem;


public class PlayerModel
{
    public readonly ReactiveProperty<InteractionMode> InteractionMode = new();

    public readonly ReactiveProperty<ICellContent> DraggingContent = new();

    public Action<GridPosition> SelectCell;
}

public enum InteractionMode
{
    Selection,
    Dragging
}


public interface IUnityUpdate
{
    void SubscribeOnUpdate(Action action);

    void UnsubscribeOnUpdate(Action action);
}

public class PlayerManager : MonoBehaviour, IUnityUpdate
{
    public PlayerModel PlayerModel { get; private set; } = new();

    public Camera RaycasterCamera;
    public LayerMask LayerMask;
    public Transform Cursor;
    public Transform DraggingRoot;

    public BuildingsConfig BuildingsConfig;

    private PlayerInputManager _playerInputManager;
    private GridManager _gridManager;
    private ViewsProvider _viewsProvider;

    public CursorController CursorController { get; private set; }
    public Raycaster Raycaster { get; private set; }
    public BuildingManager BuildingManager { get; private set; }
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
        BuildingManager = new(BuildingsConfig, _gridManager, _viewsProvider);
        DraggingContentController = new DraggingContentController(DraggingRoot, _viewsProvider);

        InitializePlayerInteractionStateMachine();
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
        var dependencies = new Dependencies();
        dependencies.Register(_playerInputManager);
        dependencies.Register(BuildingManager);
        dependencies.Register(Raycaster);
        dependencies.Register(PlayerModel);
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

    private void OnDestroy()
    {
        PlayerInteractionStateMachine?.Stop();
    }

    private void Update()
    {
        UpdateHandler?.Invoke();

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

        bool showCursor = Raycaster.TryGetCursorPositionFromScreenPoint(Input.mousePosition, out var cursorPosition);
        CursorController.SetActive(showCursor);
        if (showCursor)
        {
            CursorController.SetPosition(cursorPosition.Value);
        }*/
    }
    
    private Action? UpdateHandler;


    public void SubscribeOnUpdate(Action action)
    {
        UpdateHandler += action;
    }

    public void UnsubscribeOnUpdate(Action action)
    {
        UpdateHandler -= action;
    }
}


public class CursorController
{
    private readonly Transform cursor;

    public CursorController(Transform cursor)
    {
        this.cursor = cursor;
    }

    public void SetPosition(Vector3 position)
    {
        cursor.position = position;
    }

    public void SetActive(bool active)
    {
        cursor.gameObject.SetActive(active);
    }
}

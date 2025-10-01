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
    public GameConfigurationSo GameConfiguration;

    private PlayerInputManager _playerInputManager;
    private GridManager _gridManager;
    private ViewsProvider _viewsProvider;
    
    private Action? UpdateHandler;
    private GameSystemsInitialization _gameSystemsInitialization;
    private DependencyContainer _innerDependencies;

    private void Awake()
    {
        _playerInputManager = new PlayerInputManager();
        _viewsProvider = new ViewsProvider();

        _innerDependencies = new DependencyContainer();
        _innerDependencies.Register(RaycasterCamera);
        
        _innerDependencies.Register(GameConfiguration);
        _innerDependencies.Register(_playerInputManager);
        _innerDependencies.Register<IViewsProvider>(_viewsProvider);
        _innerDependencies.Register<IUnityUpdate>(this);
        
        InitializeGameSystems(_innerDependencies);

        WindowTest();
    }
    
    private void OnDestroy()
    {
        _viewsProvider.Dispose();
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

        var storage = _innerDependencies.Resolve<PlayerResourcesModel>();
        
        for (int i = 0; i < MaxIndex; i++)
        {
            var resourceType = (ResourceType)i;
            var amount = storage.GetResourceAmount(resourceType);
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
using Installers;
using VContainer;
using VContainer.Unity;

public class AppStartup : LifetimeScope
{
    //TODO: add app level systems, fsm
    
    public GameSystemsInstaller _installer;

    protected override void Configure(IContainerBuilder builder)
    {
        //_installer.Build();
            //_installer.CreateChild()
    }

    // public GameConfigsInstaller GameConfigsInstaller;
    //
    // public Camera RaycasterCamera;
    //
    // private GameSystemsInitialization _gameSystemsInitialization;
    // private DependencyContainer _innerDependencies;
    //
    // private void Awake()
    // {
    //     Initialized = false;
    //
    //     var unityUpdate = new GameObject("UnityUpdate").AddComponent<UnityUpdate>();
    //     DontDestroyOnLoad(unityUpdate);
    //
    //     _innerDependencies = new DependencyContainer();
    //     _innerDependencies.Register(RaycasterCamera);
    //     _innerDependencies.Register<IUnityUpdate>(unityUpdate);
    //     _innerDependencies.Register<IUnityFixedUpdate>(unityUpdate);
    //     
    //     InitializeGameSystems(_innerDependencies);
    // }
    //
    // private async void OnDestroy()
    // {
    //     await _gameSystemsInitialization.Deinit();
    // }
    //
    // private async void InitializeGameSystems(DependencyContainer dependencies)
    // {
    //     _gameSystemsInitialization = new GameSystemsInitialization(dependencies);
    //     await _gameSystemsInitialization.Init();
    //
    //     Initialized = true;
    // }
    //
    // private void Update()
    // {
    //     _gameSystemsInitialization.Update();
    // }
    //
    // public bool Initialized { get; set; }
    //
    // [CanBeNull] private DateModel _dateModel;
    // [CanBeNull] private PopulationModel _populationModel;
    // private void OnGUI()
    // {
    //     int MaxIndex = 5;
    //
    //     if (!Initialized)
    //     {
    //         return;
    //     }
    //
    //     var storage = _innerDependencies.Resolve<PlayerResourcesModel>();
    //     
    //     for (int i = 0; i < MaxIndex; i++)
    //     {
    //         var resourceType = (ResourceType)i;
    //         var amount = storage.GetResourceAmount(resourceType);
    //         GUI.Label(new Rect(20 + 50 * i, 20, 50, 50), new GUIContent($"{resourceType.ToString()}:\n{amount}"));
    //     }
    //
    //     _dateModel ??= _innerDependencies.Resolve<DateModel>();
    //     if (_dateModel != null)
    //     {
    //         GUI.Label(new Rect(20, 70, 1000, 50), new GUIContent(_dateModel.ToString()));
    //     }
    //     
    //     _populationModel ??= _innerDependencies.Resolve<PopulationModel>();
    //     if (_populationModel != null)
    //     {
    //         GUI.Label(new Rect(20, 100, 100, 50),
    //             new GUIContent($"Population: {_populationModel.CurrentPopulation.Value.ToString()} / {_populationModel.AvailableHouseholds.Value.ToString()} houses"));
    //     }
    // }
}
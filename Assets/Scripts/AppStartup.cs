using GameSystems.Implementation.GameTime;
using GameSystems.Implementation.PopulationFeature;
using Installers;
using JetBrains.Annotations;
using ResourcesSystem;
using UnityEngine;
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
  
}
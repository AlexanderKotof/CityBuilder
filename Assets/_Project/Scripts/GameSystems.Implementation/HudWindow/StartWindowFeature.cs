using System;
using CityBuilder.Dependencies;
using CityBuilder.GameSystems.Common.WindowSystem;
using CityBuilder.Network.SupabaseApi;
using CityBuilder.Views.Implementation.Windows;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.HudWindow
{
    public class StartWindowFeature : IInitializable, IDisposable
    {
        private readonly IWindowsProvider _windowsProvider;
        private readonly IAuthService _authService;
        private readonly GameSceneLoader _sceneLoader;
        private readonly IDependencyContainer _dependencies = new DependencyContainer();
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private StartWindowModel _model;
        private bool _inProcess;

        public StartWindowFeature(IWindowsProvider windowsProvider, IAuthService authService, GameSceneLoader sceneLoader)
        {
            _windowsProvider = windowsProvider;
            _authService = authService;
            _sceneLoader = sceneLoader;


            //TODO: add inner dependencies
            
            //TODO: control all world-ui should be there or smth... 
        }

        public async void Initialize()
        {
            _authService.OnAuthenticated.Subscribe(OnAuthentificated).AddTo(_disposables);
            _authService.OnError.Subscribe(OnAuthError).AddTo(_disposables);
            
            _model = await _windowsProvider.CreateWindow<StartWindowModel>(new WindowCreationData("StartWindow", 0), _dependencies);
            _model.IsActive.Value = true;
            
            _model.RegistrationSubmit
                .Subscribe(OnSubmitRegistration)
                .AddTo(_disposables);
            _model.EnterGamePressed.Subscribe(OnEnterGamePressed).AddTo(_disposables);
        }

        private void OnAuthError(Unit _) => UpdateView();

        private void OnAuthentificated(string _) => UpdateView();

        private void UpdateView()
        {
            if (_model == null)
                return;
            
            if (_model.IsActive.Value == false)
                return;

            bool isAuthenticated = _authService.IsAuthenticated();
            _model.ShowRegistration.Value = isAuthenticated == false;
            _model.ShowEnteringGame.Value = isAuthenticated;

            if (isAuthenticated)
            {
                _model.PlayerNickname.Value = _authService.GetPlayerData().display_name;
            }
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
            _windowsProvider.Recycle(_model);
        }

        private async void OnSubmitRegistration(string name)
        {
            if (_inProcess == true)
                return;
            
            _inProcess = true;
            await _authService.CreateGuestPlayer(name);
            _inProcess = false;
        }

        private void OnEnterGamePressed(Unit _)
        {
            _model.IsActive.Value = false;
            _sceneLoader.LoadGameScene().Forget();
        }
    }
}
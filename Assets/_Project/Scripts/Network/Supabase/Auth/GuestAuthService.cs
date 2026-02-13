using System;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Network.Supabase.Core;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using Logger = Network.Supabase.Core.Logger;

namespace CityBuilder.Network.SupabaseApi
{
    public class GuestAuthService : IAuthService, IInitializable, IDisposable
    {
        private readonly ISupabaseManager _manager;
        private readonly INetworkClient _networkClient;
        private readonly CompositeDisposable _disposables = new();
        private PlayerData _currentPlayerData;
        private string CurrentPlayerId => _currentPlayerData?.id ?? string.Empty;

        private Supabase.Client Client => _manager.Supabase();
        public IObservable<string> OnAuthenticated => _onAuthenticated;
        public IObservable<Unit> OnError => _onError;
        
        private Subject<string> _onAuthenticated = new();
        private Subject<Unit> _onError = new();
    
        public GuestAuthService(ISupabaseManager manager, INetworkClient networkClient)
        {
            _manager = manager;
            _networkClient = networkClient;
        }

        public void Initialize() => InitializeInternal().Forget();

        private async UniTask InitializeInternal()
        {
            await UniTask.WaitWhile(_networkClient, nc => nc.IsConnected.Value != true);
            
            string playerId = $"guest_{SystemInfo.deviceUniqueIdentifier}";
            
            var response = await Client
                .From<PlayerData>()
                .Select("*")
                .Where(x => x.id == playerId)
                .Get();

            var existingPlayerData = response.Models.FirstOrDefault();
            if (existingPlayerData != null)
            {
                _currentPlayerData = existingPlayerData;
                _onAuthenticated.OnNext(playerId);
                return;
            }
            
            _onError.OnNext(Unit.Default);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
            _onAuthenticated?.Dispose();
            _onError?.Dispose();
        }

        // public void Tick()
        // {
        //     if (_networkClient.IsConnected.Value == false)
        //         return;
        //
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         CreateGuestPlayer(TODO);
        //     }
        // }

        // private async void HelloWorldRequest()
        // {
        //     _ = await _networkClient.InvokeFunction(
        //         "hello-world",
        //         ("name", "UnityPlayer"));
        // }

        public PlayerData GetPlayerData()
        {
            return _currentPlayerData;
        }

        public async UniTask CreateGuestPlayer(string nickname)
        {
            try
            {
                if (_currentPlayerData == null)
                {
                    // Проверяем, существует ли игрок
                    var player = await CreatePlayer(nickname);
                    _currentPlayerData = player;
                    _onAuthenticated.OnNext(CurrentPlayerId);

                    Logger.Log($"[Auth] Guest login successful: {CurrentPlayerId}");
                }
                else
                {
                    _onAuthenticated.OnNext(CurrentPlayerId);
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                _onError.OnNext(Unit.Default);
            }
        }

        public void LoginWithEmail(string email, string password)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated()
        {
            return _currentPlayerData != null;
        }

        public string GetPlayerId()
        {
            return CurrentPlayerId;
        }

        private async UniTask<PlayerData> CreatePlayer(string nickname)
        {
            var createGuestUserResponse =
                await _networkClient.InvokeFunction<Response<PlayerData>>("auth-guest", ("device_id", SystemInfo.deviceUniqueIdentifier), ("display_name", nickname));
            return createGuestUserResponse.payload;
        }
    }
}
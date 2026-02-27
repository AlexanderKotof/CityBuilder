using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Network.Supabase.Core;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using Logger = Utilities.Logger;

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
        
        private readonly Subject<string> _onAuthenticated = new();
        private readonly Subject<Unit> _onError = new();
    
        public GuestAuthService(ISupabaseManager manager, INetworkClient networkClient)
        {
            _manager = manager;
            _networkClient = networkClient;

            _onAuthenticated.AddTo(_disposables);
            _onError.AddTo(_disposables);
        }

        public void Initialize() => InitializeInternal().Forget();

        private async UniTask InitializeInternal()
        {
            await UniTask.WaitWhile(_networkClient, nc => nc.IsConnected.Value != true);
            
            string playerId = $"guest_{SystemInfo.deviceUniqueIdentifier}";
            
            var player = await GetPlayerData(playerId);

            var existingPlayerData = player;
            if (existingPlayerData != null)
            {
                _currentPlayerData = existingPlayerData;
                _onAuthenticated.OnNext(playerId);

                Logger.Log($"Successfully loaded player data: {CurrentPlayerId}");
                return;
            }
            
            Logger.Log($"User not founded, proceeding to initial user creation");
            _onError.OnNext(Unit.Default);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }

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
                    var player = await CreatePlayer(nickname);
                    _currentPlayerData = player;
                    _onAuthenticated.OnNext(CurrentPlayerId);

                    Logger.Log($"Created player with id: {CurrentPlayerId}");
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

        private async UniTask<PlayerData> GetPlayerData(string id)
        {
            var getPlayerDataResponse =
                await _networkClient.InvokeFunction<Response<PlayerData>>("player-get", ("player_id", id));
            return getPlayerDataResponse.payload;
        }

        private async UniTask<PlayerData> CreatePlayer(string nickname)
        {
            var createGuestUserResponse =
                await _networkClient.InvokeFunction<Response<PlayerData>>("auth-guest", ("device_id", SystemInfo.deviceUniqueIdentifier), ("display_name", nickname));
            return createGuestUserResponse.payload;
        }
    }
}
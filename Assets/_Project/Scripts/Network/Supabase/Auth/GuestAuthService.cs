using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.example;
using Postgrest;
using Supabase;
using UniRx;
using UnityEngine;
using VContainer.Unity;
using Logger = com.example.Logger;
using Random = UnityEngine.Random;

namespace CityBuilder.Network.SupabaseApi
{
    public class GuestAuthService : IAuthService, ITickable, IDisposable
    {
        private readonly SupabaseManager _manager;
        private readonly INetworkClient _networkClient;
        private readonly CompositeDisposable _disposables = new();
        private string _currentPlayerId;

        private Supabase.Client Client => _manager.Supabase();
        public IObservable<string> OnAuthenticated => _onAuthenticated;
        public IObservable<Unit> OnError => _onError;
        
        private Subject<string> _onAuthenticated = new();
        private Subject<Unit> _onError = new();
    
        public GuestAuthService(SupabaseManager manager, INetworkClient networkClient)
        {
            _manager = manager;
            _networkClient = networkClient;
        }
        
        public void Tick()
        {
            if (_manager.IsConnected.Value == false)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoginAsGuest();
            }
        }

        private async void HelloWorldRequest()
        {
            _ = await _networkClient.InvokeFunction(
                "hello-world",
                ("name", "UnityPlayer"));
        }

        public async void LoginAsGuest()
        {
            try
            {
                // Проверяем, существует ли игрок
                var player = await GetOrCreatePlayer();
            
                _currentPlayerId = player?.id ?? string.Empty;
                _onAuthenticated.OnNext(_currentPlayerId);
            
                Logger.Log($"[Auth] Guest login successful: {_currentPlayerId}");
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
            throw new NotImplementedException();
        }

        public string GetPlayerId()
        {
            throw new NotImplementedException();
        }

        private async Task<PlayerData> GetOrCreatePlayer()
        {
            // Генерируем уникальный ID для гостя
            string playerId = $"guest_{SystemInfo.deviceUniqueIdentifier}";
            
            var response = await Client
                .From<PlayerData>()
                .Select("*")
                .Where(x => x.id == playerId)
                .Get();

            var playerData = response.Models.FirstOrDefault();
            if (playerData == null)
            {
                var getOrCreateGuestUser =
                    await _networkClient.InvokeFunction<Response<PlayerData>>("auth-guest", ("device_id", SystemInfo.deviceUniqueIdentifier), ("display_name", $"Guest_{UnityEngine.Random.Range(1000, 9999)}"));
                return getOrCreateGuestUser.payload;
            }
            
            // playerData.
            //
            // // Обновляем время последнего входа
            // await Client
            //     .From<players>()
            //     .Where(x => x.id == playerId)
            //     .Update(new QueryOptions()):

            return playerData;
        }
    
        public void Dispose()
        {
            _disposables.Dispose();
            _onAuthenticated?.Dispose();
            _onError?.Dispose();
        }
    }
}
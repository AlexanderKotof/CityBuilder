using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.example;
using Supabase;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.Network.SupabaseApi
{
    public class GuestAuthService : IAuthService, ITickable, IDisposable
    {
        private readonly SupabaseManager _manager;
        private readonly INetworkClient _networkClient;
        private readonly Supabase.Client _client;
        private readonly CompositeDisposable _disposables = new();
        private string _currentPlayerId;

        public IObservable<string> OnAuthenticated => _onAuthenticated;
        public IObservable<Unit> OnError => _onError;
        
        private Subject<string> _onAuthenticated = new();
        private Subject<Unit> _onError = new();
    
        public GuestAuthService(SupabaseManager manager, INetworkClient networkClient)
        {
            _manager = manager;
            _networkClient = networkClient;
            _client = manager.Supabase();
        }
        
        public void Tick()
        {
            if (_manager.IsConnected.Value == false)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HelloWorldRequest();
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
                // Генерируем уникальный ID для гостя
                string guestId = $"{AuthConfig.GuestPrefix}{SystemInfo.deviceUniqueIdentifier}";
            
                // Проверяем, существует ли игрок
                var player = await GetOrCreatePlayer(guestId);
            
                _currentPlayerId = player.Id;
                _onAuthenticated.OnNext(_currentPlayerId);
            
                Debug.Log($"[Auth] Guest login successful: {_currentPlayerId}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Auth] Guest login failed: {e.Message}");
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

        private async Task<PlayerData> GetOrCreatePlayer(string playerId)
        {
            // var response = await _client
            //     .From<PlayerData>(AuthConfig.PlayersTableKey)
            //     .Select("*")
            //     .Where(data => string.Equals(data.Id, playerId))
            //     .Get();
            //
            // if (response.Models.Count == 0)
            // {
            //     // Создаём нового игрока
            //     var newPlayer = new PlayerData
            //     {
            //         Id = playerId,
            //         CreatedAt = DateTime.UtcNow,
            //         DisplayName = $"Guest_{UnityEngine.Random.Range(1000, 9999)}",
            //         Level = 1,
            //         Score = 0
            //     };
            //
            //     await _client
            //         .From(AuthConfig.PlayersTableKey)
            //         .Insert(newPlayer);
            //     
            //     return newPlayer;
            // }
            //
            // // Обновляем время последнего входа
            // await _client
            //     .From(AuthConfig.PlayersTableKey)
            //     .Update(new { last_login = DateTime.UtcNow })
            //     .Eq("id", playerId)
            //     .Execute();
            //
            // return response.Models[0].ToObject<PlayerData>();
            throw new NotImplementedException();

        }
    
        public void Dispose()
        {
            _disposables.Dispose();
            _onAuthenticated?.Dispose();
            _onError?.Dispose();
        }
    }
}
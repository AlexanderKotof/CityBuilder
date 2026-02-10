using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace CityBuilder.Network.Auth
{
    public static class AuthConfig
    {
        public const string SUPABASE_URL = "https://wjdtovuqkvglslwpfbxd.supabase.co";
        public const string ANON_KEY = "sb_publishable_N8k_S2ZcP1pG527mcnPDoQ_5DIFUU-6";
        public const string GUEST_PREFIX = "guest_";
    
        // Таблицы
        public const string PLAYERS_TABLE = "players";
        public const string SESSIONS_TABLE = "sessions";
    }
    
    public interface IAuthService
{
    IObservable<string> OnAuthenticated { get; }
    IObservable<Unit> OnError { get; }
    
    void LoginAsGuest();
    void LoginWithEmail(string email, string password);
    void Logout();
    bool IsAuthenticated();
    string GetPlayerId();
}

// public class GuestAuthService : IAuthService, IDisposable
// {
//     private readonly Supabase.Client _client;
//     private readonly CompositeDisposable _disposables = new();
//     private string _currentPlayerId;
//     
//     public IObservable<string> OnAuthenticated { get; }
//     public IObservable<Unit> OnError { get; }
//     private Subject<string> _onAuthenticated = new();
//     private Subject<Unit> _onError = new();
//     
//     public GuestAuthService()
//     {
//         OnAuthenticated = _onAuthenticated.AsObservable();
//         OnError = _onError.AsObservable();
//         
//         var options = new SupabaseOptions
//         {
//             AutoRefreshToken = true,
//             AutoConnectRealtime = true
//         };
//         
//         _client = new Supabase.Client(
//             AuthConfig.SUPABASE_URL, 
//             AuthConfig.ANON_KEY, 
//             options
//         );
//     }
//     
//     public async void LoginAsGuest()
//     {
//         try
//         {
//             // Генерируем уникальный ID для гостя
//             string guestId = $"{AuthConfig.GUEST_PREFIX}{SystemInfo.deviceUniqueIdentifier}";
//             
//             // Проверяем, существует ли игрок
//             var player = await GetOrCreatePlayer(guestId);
//             
//             _currentPlayerId = player.Id;
//             _onAuthenticated.OnNext(_currentPlayerId);
//             
//             Debug.Log($"[Auth] Guest login successful: {_currentPlayerId}");
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"[Auth] Guest login failed: {e.Message}");
//             _onError.OnNext(Unit.Default);
//         }
//     }
//     
//     private async Task<PlayerData> GetOrCreatePlayer(string playerId)
//     {
//         var response = await _client
//             .From(AuthConfig.PLAYERS_TABLE)
//             .Select("*")
//             .Eq("id", playerId)
//             .Get();
//             
//         if (response.Models.Count == 0)
//         {
//             // Создаём нового игрока
//             var newPlayer = new PlayerData
//             {
//                 Id = playerId,
//                 CreatedAt = DateTime.UtcNow,
//                 LastLogin = DateTime.UtcNow,
//                 DisplayName = $"Guest_{Random.Range(1000, 9999)}",
//                 Level = 1,
//                 XP = 0
//             };
//             
//             await _client
//                 .From(AuthConfig.PLAYERS_TABLE)
//                 .Insert(newPlayer);
//                 
//             return newPlayer;
//         }
//         
//         // Обновляем время последнего входа
//         await _client
//             .From(AuthConfig.PLAYERS_TABLE)
//             .Update(new { last_login = DateTime.UtcNow })
//             .Eq("id", playerId)
//             .Execute();
//             
//         return response.Models[0].ToObject<PlayerData>();
//     }
//     
//     public void Dispose()
//     {
//         _disposables.Dispose();
//         _onAuthenticated?.Dispose();
//         _onError?.Dispose();
//     }
//     
//     // ... остальные методы интерфейса
// }
}
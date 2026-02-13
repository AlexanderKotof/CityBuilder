using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace CityBuilder.Network.SupabaseApi
{
    public interface IAuthService
    {
        IObservable<string> OnAuthenticated { get; }
        IObservable<Unit> OnError { get; }
        PlayerData GetPlayerData();
        UniTask CreateGuestPlayer(string nickname);
        void LoginWithEmail(string email, string password);
        void Logout();
        bool IsAuthenticated();
        string GetPlayerId();
    }
}
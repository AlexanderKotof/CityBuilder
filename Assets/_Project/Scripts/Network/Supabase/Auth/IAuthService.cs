using System;
using UniRx;

namespace CityBuilder.Network.SupabaseApi
{
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
}
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using TMPro;
using UnityEngine;
namespace com.example
{
	public class SessionListener
	{
		public void UnityAuthListener(IGotrueClient<User, Session> sender, Constants.AuthState newState)
		{
			LogSignedAs(sender);

			switch (newState)
			{
				case Constants.AuthState.SignedIn:
					Debug.Log("Signed In");
					break;
				case Constants.AuthState.SignedOut:
					Debug.Log("Signed Out");
					break;
				case Constants.AuthState.UserUpdated:
					Debug.Log("Signed In");
					break;
				case Constants.AuthState.PasswordRecovery:
					Debug.Log("Password Recovery");
					break;
				case Constants.AuthState.TokenRefreshed:
					Debug.Log("Token Refreshed");
					break;
				case Constants.AuthState.Shutdown:
					Debug.Log("Shutdown");
					break;
				default:
					Debug.Log("Unknown Auth State Update");
					break;
			}
		}

		private static void LogSignedAs(IGotrueClient<User, Session> sender)
		{
			if (sender.CurrentUser?.Email == null)
				Debug.Log("No user logged in");
			else
			{
				Debug.Log($"Logged in as {sender.CurrentUser.Email}");
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CityBuilder.Network.SupabaseApi;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Supabase;
using Supabase.Gotrue;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Client = Supabase.Client;

namespace com.example
{
	public class SupabaseManager : MonoBehaviour, IDisposable, IAsyncStartable, INetworkClient
	{
		[Inject]
		private readonly SessionListener _sessionListener;

		// Public in case other components are interested in network status
		private readonly NetworkStatus _networkStatus = new();

		// Internals
		private Client? _client;

		public Client? Supabase() => _client;
		
		public IReadOnlyReactiveProperty<bool> IsConnected => _isConnected;

		private readonly ReactiveProperty<bool> _isConnected = new();

		public async UniTask StartAsync(CancellationToken cancellation = default)
		{
			Logger.Log("Starting Supabase...");

			await Connect();
		}

		private async Task Connect()
		{
			SupabaseOptions options = new();
			// We set an option to refresh the token automatically using a background thread.
			options.AutoRefreshToken = true;

			// We start setting up the client here
			Client client = new(AuthConfig.Url, AuthConfig.AnonKey, options);

			// The first thing we do is attach the debug listener
			client.Auth.AddDebugListener(DebugListener!);

			// Next we set up the network status listener and tell it to turn the client online/offline
			_networkStatus.Client = (Supabase.Gotrue.Client)client.Auth;

			// Next we set up the session persistence - without this the client will forget the session
			// each time the app is restarted
			client.Auth.SetPersistence(new UnitySession());

			// This will be called whenever the session changes
			client.Auth.AddStateChangedListener(_sessionListener.UnityAuthListener);

			Logger.Log("Loading session...");
			// Fetch the session from the persistence layer
			// If there is a valid/unexpired session available this counts as a user log in
			// and will send an event to the UnityAuthListener above.
			client.Auth.LoadSession();

			// Allow unconfirmed user sessions. If you turn this on you will have to complete the
			// email verification flow before you can use the session.
			client.Auth.Options.AllowUnconfirmedUserSessions = true;

			// We check the network status to see if we are online or offline using a request to fetch
			// the server settings from our project. Here's how we build that URL.
			string url = $"{AuthConfig.Url}/auth/v1/settings?apikey={AuthConfig.AnonKey}";
			try
			{
				// This will get the current network status
				client.Auth.Online = await _networkStatus.StartAsync(url);
			}
			catch (NotSupportedException notSupportedException)
			{
				// Some platforms don't support network status checks, so we just assume we are online
				client.Auth.Online = true;
				Logger.LogException(notSupportedException);
			}
			catch (Exception e)
			{
				// Something else went wrong, so we assume we are offline
				Logger.LogException(e, gameObject);
				client.Auth.Online = false;
			}
			
			if (client.Auth.Online)
			{
				// Now we start up the client, which will in turn start up the background thread.
				// This will attempt to refresh the session token, which in turn may send a second
				// user login event to the UnityAuthListener.
				await client.InitializeAsync();

				// Here we fetch the server settings and log them to the console
				Settings serverConfiguration = (await client.Auth.Settings())!;
				Logger.Log($"Auto-confirm emails on this server: {serverConfiguration.MailerAutoConfirm}");

				_isConnected.Value = true;
				
				Logger.Log($"Successfully connected to server: {url}");
			}
			else
			{
				_isConnected.Value = false;
				Logger.LogError($"Cannot connect to server: {url}");
			}
			_client = client;
		}

		public async UniTask TryReconnect()
		{
			if (_client != null)
			{
				_client?.Auth.Shutdown();
				_client = null;
			}
			
			await Connect();
		}

		private void DebugListener(string message, Exception e)
		{
			Logger.Log(message, gameObject);
			
			if (e != null)
				Logger.LogException(e, gameObject);
		}

		// This is called when Unity shuts down. You want to be sure to include this so that the
		// background thread is terminated cleanly. Keep in mind that if you are running the app
		// in the Unity Editor, if you don't call this method you will leak the background thread!
		private void OnApplicationQuit()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (_client != null)
			{
				_client.Auth.Shutdown();
				_client.Auth.RemoveStateChangedListener(_sessionListener.UnityAuthListener);
				_client = null;
			}
			_isConnected.Dispose();
		}
		
		public async UniTask<string> InvokeFunction(string function, Dictionary<string, string> header = null, Dictionary<string, object> body = null)
		{
			if (_isConnected.Value == false)
			{
				Logger.LogError("Client is not connected");
				return null;
			}

			var payload = header != null || body != null ? 
				new Supabase.Functions.Client.InvokeFunctionOptions
				{
					Headers = header ?? new Dictionary<string, string>(),
					Body = body ?? new Dictionary<string, object>(),
				} 
				: null;
			var response = await _client!.Functions.Invoke(
				function,
				AuthConfig.AnonKey,
				payload);
			Logger.Log(response);
			return response;
		}

		public async UniTask<string> InvokeFunction(string function, params (string, object)[] body)
		{
			if (_isConnected.Value == false)
			{
				Logger.LogError("Client is not connected");
				return null;
			}

			var payload = body != null ? 
				new Supabase.Functions.Client.InvokeFunctionOptions
				{
					Body = body.ToDictionary(p => p.Item1, p => p.Item2),
				} 
				: null;
			var response = await _client!.Functions.Invoke(
				function,
				AuthConfig.AnonJwtKey,
				payload);
			Logger.Log(response);
			return response;
		}

		public async UniTask<TResponse> InvokeFunction<TResponse>(string function, params (string, object)[] body) where TResponse : Response
		{
			var responce = await InvokeFunction(function, body);
			return JsonConvert.DeserializeObject<TResponse>(responce);
		}
	}
}

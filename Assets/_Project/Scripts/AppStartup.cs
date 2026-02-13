using Cysharp.Threading.Tasks;
using Network.Supabase.Core;
using UnityEngine;
using VContainer.Unity;
using Logger = Network.Supabase.Core.Logger;

namespace CityBuilder
{
    /// <summary>
    /// This is entry point of all application, creates main scope
    /// </summary>
    public class AppStartup : MonoBehaviour
    {
        public const string MainScopeName = "MainScope";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            Logger.LogWarning("Initializing AppStartup");
            Startup().Forget();
        }

        private static UniTask Startup()
        {
            var mainScope = LifetimeScope.Create(new CommonAppSystemsInstaller(), name: MainScopeName);
            DontDestroyOnLoad(mainScope.gameObject);
            return UniTask.CompletedTask;
        }

        private static TScope InstallScope<TScope>(LifetimeScope mainScope) where TScope : LifetimeScope
        {
            var scope = mainScope.CreateChild<TScope>(childScopeName: typeof(TScope).Name);
            //scope.Build();
            return scope;
        }
    }
}
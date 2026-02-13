using CityBuilder.Installers;
using Cysharp.Threading.Tasks;
using Network.Supabase.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CityBuilder
{
    /// <summary>
    /// This is entry point of all application, but now it's only marker for main game installer (VContainer, see Scripts/Installers folder) 
    /// </summary>
    public class AppStartup : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            Debug.LogWarning("Initializing AppStartup");
        }
        
        //TODO: add app level systems, fsm
        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            Startup().Forget();
        }

        private UniTask Startup()
        {
            var mainScope = LifetimeScope.Create(new CommonAppSystemsInstaller(), name: "MainScope");
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
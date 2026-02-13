using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.HudWindow
{
    public class GameSceneLoader
    {
        private const string GameSceneName = "GameScene";
        private const string StartSceneName = "StartScene";

        private readonly LifetimeScope _parentScope;

        public GameSceneLoader(LifetimeScope parentScope)
        {
            _parentScope = parentScope;
        }
        
        public async UniTask LoadGameScene()
        {
            await LoadScene(GameSceneName);

            // var scope = Object.FindAnyObjectByType<GameConfigsInstaller>();
            // scope.Build();
        }

        public UniTask LoadStartGameScene()
        {
            return LoadScene(StartSceneName);
        }

        private static async UniTask LoadScene(string sceneName)
        {
            var currentScene = SceneManager.GetActiveScene();
            if (string.Equals(currentScene.name, sceneName))
            {
                Debug.LogError($"{sceneName} already loaded");
                return;
            }
            
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).ToUniTask();
        }
    }
}
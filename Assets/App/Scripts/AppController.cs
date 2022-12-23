using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace TicTacToe
{
    /// <summary>
    /// App Controller
    /// <remarks>Bootloader and overall controller of app</remarks>
    /// </summary>
    public class AppController : MonoBehaviour
    {
        [SerializeField] PlayerConfig[] playerConfigs;
        
        void Start()
        {
            // Bootload game from here
            RunGameplayScreenAsync().Forget();
        }

        /// <summary>
        /// Gameplay screen
        /// </summary>
        async UniTask RunGameplayScreenAsync()
        {
            Assert.IsTrue(playerConfigs.Length == 2, "Game requires two players");
            
            // load gameplay scene and keep bootloader scene alive for scripts that we want to keep alive for app's duration
            const string SceneName = "GameplayScene";
            await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            Scene gameplayScene = SceneManager.GetSceneByName(SceneName);
            SceneManager.SetActiveScene(gameplayScene);
            
            var sceneController = FindObjectOfType<GameplayScreenController>();
            
            // run scene logic
            sceneController.Load();
            await sceneController.RunAsync(playerConfigs);
            
            throw new NotSupportedException("Game is not supposed to end");
        }
    }
}

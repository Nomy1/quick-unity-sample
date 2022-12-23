using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToe
{
    /// <summary>
    /// App Controller
    /// <remarks>Bootloader and overall controller of app</remarks>
    /// </summary>
    public class AppController : MonoBehaviour
    {
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
            // load gameplay scene and keep bootloader scene alive for scripts that we want to keep alive for app's duration
            await SceneManager.LoadSceneAsync("GameplayScene", LoadSceneMode.Additive);

            // run gameplay scene logic
            var sceneController = FindObjectOfType<GameplayController>();
            await sceneController.RunAsync();
            
            throw new NotSupportedException("Game is not supposed to end");
        }
    }
}

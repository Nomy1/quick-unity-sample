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
        
        private void Start()
        {
            // Bootload game from here
            RunGameplayScreenAsync().Forget();
        }

        /// <summary>
        /// Gameplay screen
        /// </summary>
        private async UniTask RunGameplayScreenAsync()
        {
            Assert.IsTrue(playerConfigs.Length == 2, "Game requires two players");
            
            // load gameplay scene and keep bootloader scene alive for scripts that we want to keep alive for app's duration
            const string SceneName = "GameplayScene";
            await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            
            // spawn new objects in gameplay scene
            Scene gameplayScene = SceneManager.GetSceneByName(SceneName);
            SceneManager.SetActiveScene(gameplayScene);
            
            var sceneController = FindObjectOfType<GameplayScreenController>();
            
            // load scene logic
            GameStateJson savedGameState = RetrieveSavedGameState();
            sceneController.Load(savedGameState);
            
            // run scene logic
            await sceneController.RunAsync(playerConfigs);
            
            throw new NotSupportedException("Game is not supposed to end");
        }
        
        /// <summary>
        /// Retrieve saved game state if it exists
        /// </summary>
        /// <returns></returns>
        private GameStateJson RetrieveSavedGameState()
        {
            string savedGameData = PlayerPrefs.GetString(Const.Key.SavedGame);
            
            if (!string.IsNullOrEmpty(savedGameData))
            {
                try
                {
                    return JsonUtility.FromJson<GameStateJson>(savedGameData);
                } 
                catch (Exception e)
                {
                    Debug.LogError($"Unable to deserialize saved game: {e.Message}");
                }
            }

            return null;
        }
    }
}

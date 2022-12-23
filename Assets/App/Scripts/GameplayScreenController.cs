using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    /// <summary>
    /// Gameplay Screen Controller
    /// </summary>
    public class GameplayScreenController : MonoBehaviour
    {
        [SerializeField] GameObject boardPrefab;
        
        [Header("Buttons")]
        [SerializeField] Button restartButton;
        
        [Header("Winner")]
        [SerializeField] GameObject winnerPanel;
        [SerializeField] TMP_Text winnerText;

        GameStateJson savedGameState;
        
        
        public void Load(GameStateJson savedGameState)
        {
            this.savedGameState = savedGameState;
            
            restartButton.gameObject.SetActive(false);
            winnerText.gameObject.SetActive(false);
            winnerPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// Runs screen logic
        /// </summary>
        /// <param name="playerConfigs">Participating player configuration data. First index player goes first</param>
        public async UniTask RunAsync(PlayerConfig[] playerConfigs)
        {
            // saved data gets first player priority
            var startingPlayer = savedGameState != null ? 
                (PlayerType)savedGameState.PlayerTurnIndex : 
                PlayerType.Player1;
            
            // game is never supposed to end
            while (true)
            {
                // create board each match
                GameObject boardInstance = Instantiate(boardPrefab);
                var boardView = boardInstance.GetComponent<BoardView>();
                boardView.Init(playerConfigs, savedGameState);
                
                // saved state only used for initial round
                savedGameState = null;
                
                // play one round
                PlayerType winner = await PlayRoundAsync(boardView, startingPlayer);
                
                // clear saved data after winner found
                SaveEmptyBoardState(boardView);
                
                DisplayWinner(winner, playerConfigs);
                
                // wait for input to start next round
                restartButton.gameObject.SetActive(true);
                await restartButton.onClick.OnInvokeAsync(default(CancellationToken));

                CleanupRound(boardView);
            }
        }

        private async UniTask<PlayerType> PlayRoundAsync(BoardView board, PlayerType startingPlayer)
        {
            PlayerType playerTurn = startingPlayer;
            PlayerType winner  = PlayerType.None;
                
            while (winner == PlayerType.None)
            {
                SaveBoardState(playerTurn, board);
                
                await board.WaitForTurn(playerTurn);

                winner = board.GetWinner();

                if (board.IsTie())
                {
                    // force round to end in case of tie with no winner
                    winner = PlayerType.None;
                    break;
                }
                
                // change turns
                playerTurn = playerTurn == PlayerType.Player1 ? 
                    PlayerType.Player2 : 
                    PlayerType.Player1;
            }

            return winner;
        }

        /// <summary>
        /// Save current board state
        /// </summary>
        /// <param name="currentPlayerTurn">Current player turn</param>
        /// <param name="board">Board</param>
        private void SaveBoardState(PlayerType currentPlayerTurn, BoardView board)
        {
            var gameState = new GameStateJson
            {
                BoardSaveState = board.GetBoardState(),
                PlayerTurnIndex = (int)currentPlayerTurn,
            };
            
            string jsonData = JsonUtility.ToJson(gameState);
            PlayerPrefs.SetString(Const.Key.SavedGame, jsonData);
            PlayerPrefs.Save();
        }

        private void SaveEmptyBoardState(BoardView board)
        {
            var gameState = new GameStateJson
            {
                BoardSaveState = board.GetEmptyBoardState(),
                PlayerTurnIndex = 0,
            };
            
            string jsonData = JsonUtility.ToJson(gameState);
            PlayerPrefs.SetString(Const.Key.SavedGame, jsonData);
            PlayerPrefs.Save();
        }
        
        private void DisplayWinner(PlayerType winner, PlayerConfig[] playerConfigs)
        {
            if (winner == PlayerType.None)
            {
                winnerText.text = "It's a tie!";
            } 
            else
            {
                string winnerName = winner == PlayerType.Player1 ? playerConfigs[0].Name : playerConfigs[1].Name;
                winnerText.text = $"{winnerName} Wins!";
            }
            
            winnerText.gameObject.SetActive(true);
            winnerPanel.gameObject.SetActive(true);
        }
        
        private void CleanupRound(BoardView board)
        {
            Destroy(board.gameObject);
            restartButton.gameObject.SetActive(false);
            winnerText.gameObject.SetActive(false);
            winnerPanel.gameObject.SetActive(false);
        }
    }
}


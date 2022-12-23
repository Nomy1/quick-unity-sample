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

        
        public void Load()
        {
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
            // game is never supposed to end
            while (true)
            {
                // create board each match
                GameObject boardInstance = Instantiate(boardPrefab);
                var boardView = boardInstance.GetComponent<BoardView>();
                boardView.Init(playerConfigs);

                // play one round
                PlayerType winner = await PlayRoundAsync(boardView);

                DisplayWinner(winner, playerConfigs);
                
                // wait for input to start next round
                restartButton.gameObject.SetActive(true);
                await restartButton.onClick.OnInvokeAsync(default(CancellationToken));

                CleanupRound(boardView);
            }
        }

        async UniTask<PlayerType> PlayRoundAsync(BoardView board)
        {
            PlayerType playerTurn = PlayerType.Player1;
            PlayerType winner  = PlayerType.None;
                
            while (winner == PlayerType.None)
            {
                await board.WaitForTurn(playerTurn);

                winner = board.GetWinner();

                // change turns
                playerTurn = playerTurn == PlayerType.Player1 ? 
                    PlayerType.Player2 : 
                    PlayerType.Player1;
            }

            return winner;
        }
        
        void DisplayWinner(PlayerType winner, PlayerConfig[] playerConfigs)
        {
            string winnerName = winner == PlayerType.Player1 ? playerConfigs[0].Name : playerConfigs[1].Name;
            winnerText.text = $"{winnerName}Wins!";
            winnerText.gameObject.SetActive(true);
            winnerPanel.gameObject.SetActive(true);
        }
        
        void CleanupRound(BoardView board)
        {
            Destroy(board.gameObject);
            restartButton.gameObject.SetActive(false);
            winnerText.gameObject.SetActive(false);
            winnerPanel.gameObject.SetActive(false);
        }
    }
}


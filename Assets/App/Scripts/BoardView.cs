using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace TicTacToe
{
    /// <summary>
    /// Board View
    /// <remarks>Holds and controls each tile</remarks>
    /// </summary>
    public class BoardView : MonoBehaviour
    {
        [SerializeField] TileView[] tileViews;
        
        PlayerConfig[] playerConfigs;

        /// <summary>
        /// Initialize board with participating players
        /// </summary>
        /// <param name="playerConfigs">Participating player configurations</param>
        public void Init(PlayerConfig[] playerConfigs, GameStateJson gameState = null)
        {
            this.playerConfigs = playerConfigs;

            // load game state
            if (gameState != null)
            {
                for(int i=0; i<gameState.BoardSaveState.Length; i++)
                {
                    var owner = (PlayerType)gameState.BoardSaveState[i];

                    // only set tiles with an owner
                    if (owner != PlayerType.None)
                    {
                        var config = owner == PlayerType.Player1 ? playerConfigs[0] : playerConfigs[1];
                        tileViews[i].SetOwner(config, owner);
                    }
                }
            }
        }

        /// <summary>
        /// Wait for player to finish turn
        /// </summary>
        /// <param name="playerIndex">Current player's turn index</param>
        public async UniTask WaitForTurn(PlayerType player)
        {
            // listen to tile selection
            for (var i = 0; i < tileViews.Length; i++)
            {
                var tile = tileViews[i];
                
                // only allow unselected tiles to be updated
                if (tile.OwnerPlayer == PlayerType.None)
                {
                    tile.SetTurnOwner(playerConfigs[(int)player], player);
                    tile.OnMouseClick += OnTileSelected;
                }
            }

            bool isTurnDone = false;
            
            // wait until player selects a tile
            await UniTask.WaitWhile(() => !isTurnDone);

            // stop listening
            foreach (var tile in tileViews)
            {
                tile.OnMouseClick -= OnTileSelected;
            }
            

            void OnTileSelected()
            {
                isTurnDone = true;
            }
        }

        /// <summary>
        /// Get winner
        /// </summary>
        /// <returns>Player who won round</returns>
        public PlayerType GetWinner()
        {
            // check rows
            for (int i = 0; i < tileViews.Length; i += 3)
            {
                var owner = GetLineOwner(tileViews[i], tileViews[i + 1], tileViews[i + 2]);
                if (owner != PlayerType.None)
                    return owner;
            }
           
            // check columns
            for (int i = 0; i < 3; i++)
            {
                var owner = GetLineOwner(tileViews[i], tileViews[i + 3], tileViews[i + 6]);
                if (owner != PlayerType.None)
                    return owner;
            }
            
            // check diagonal #1
            var diag1Owner = GetLineOwner(tileViews[0], tileViews[4], tileViews[8]);
            if (diag1Owner != PlayerType.None)
                return diag1Owner;
            
            // check diagonal #2
            var diag2Owner = GetLineOwner(tileViews[2], tileViews[4], tileViews[6]);
            if (diag2Owner != PlayerType.None)
                return diag2Owner;

            return PlayerType.None;
        }

        private PlayerType GetLineOwner(params TileView[] lineTiles)
        {
            Assert.IsTrue(lineTiles.Length == 3, "Only 3x3 boards supported");
            
            if (lineTiles[0].OwnerPlayer == lineTiles[1].OwnerPlayer && 
                lineTiles[1].OwnerPlayer == lineTiles[2].OwnerPlayer)
            {
                return lineTiles[0].OwnerPlayer;
            }

            return PlayerType.None;
        }
        
        /// <summary>
        /// Is the game a tie?
        /// </summary>
        public bool IsTie()
        {
            return tileViews.All(t => t.OwnerPlayer != PlayerType.None);
        }
        
        public int[] GetBoardState()
        {
            return tileViews.Select(t => (int)t.OwnerPlayer).ToArray();
        }

        public int[] GetEmptyBoardState()
        {
            return tileViews.Select(t => -1).ToArray();
        }
    }
}
using UnityEngine;

namespace TicTacToe
{
    /// <summary>
    /// Player configuration
    /// </summary>
    [CreateAssetMenu(fileName = "new player config", menuName = "Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        /// <summary>
        /// Player Name
        /// <remarks>Defaults to Player X if empty</remarks>
        /// </summary>
        public string Name;
        
        /// <summary>
        /// Player's team color
        /// </summary>
        public Color TeamColor;
    }
}


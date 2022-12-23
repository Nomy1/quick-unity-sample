using System;
using Newtonsoft.Json;

[Serializable]
public class GameStateJson
{
    /// <summary>
    /// Current player turn
    /// </summary>
    [JsonRequired, JsonProperty("player_turn_index")]
    public int PlayerTurnIndex;
    
    /// <summary>
    /// State of board
    /// </summary>
    [JsonRequired, JsonProperty("player_turn_index")]
    public int[] BoardSaveState;
}

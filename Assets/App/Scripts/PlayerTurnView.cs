using TicTacToe;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnView : MonoBehaviour
{
    [SerializeField] Image meshRenderer;
    [SerializeField] TMP_Text nameText;
    
    public void Init(PlayerConfig config)
    {
        meshRenderer.color = config.TeamColor;
        nameText.text = config.Name;
    }

    public void Display() => gameObject.SetActive(true);
    
    public void Hide() => gameObject.SetActive(false);
}

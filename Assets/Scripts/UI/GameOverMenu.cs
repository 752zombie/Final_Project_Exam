using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gameOverMessage;

    public void SetGameOverMessage(string message)
    {
        gameOverMessage.text = message;
    }
}

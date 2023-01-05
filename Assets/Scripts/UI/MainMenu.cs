using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The scene to load when starting a new game.")]
    private int NewGameScene = 1;

    public void NewGame()
    {
        PersistenceManager.Instance.NewGame();
        LevelManager.Instance.LoadLevel(NewGameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private MenuNavigator menuNavigator;

    public void Resume()
    {
        menuNavigator.CloseMenu();
    }

    public void QuitToMainMenu()
    {
        LevelManager.Instance.LoadMainMenu();
    }


}

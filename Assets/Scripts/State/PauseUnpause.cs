using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUnpause : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnPauseMenuClosed += Unpause;
    }

    private void OnDisable()
    {
        EventManager.OnPauseMenuClosed -= Unpause;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameStateManager.IsPaused)
        {
            Pause();
        }
    }

    private void Pause()
    {
        GameStateManager.PauseGame();
        EventManager.OpenPauseMenu();
    }

    private void Unpause()
    {
        GameStateManager.UnpauseGame();
    }
}

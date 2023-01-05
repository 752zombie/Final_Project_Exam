using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    MainMenu,
    Exploration,
    Combat,
    Cutscene
}

public class GameStateManager : MonoBehaviour
{
    public static bool IsPaused = false;
    public static GameState CurrentState;


    public static void PauseGame()
    {
        Time.timeScale = 0;
        IsPaused = true;
    }

    public static void UnpauseGame()
    {
        Time.timeScale = 1;
        IsPaused = false;
    }
}

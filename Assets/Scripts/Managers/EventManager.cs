using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<string> OnGameOver;
    public static event Action OnOpenPauseMenu;
    public static event Action OnPauseMenuClosed;

    public static void SendGameOver(string message)
    {
        OnGameOver?.Invoke(message);
    }

    public static void OpenPauseMenu()
    {
        OnOpenPauseMenu?.Invoke();
    }

    public static void OnPauseMenuClosedInvoke()
    {
        OnPauseMenuClosed?.Invoke();
    }

}

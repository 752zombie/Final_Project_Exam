using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private MenuNavigator GameOverScreen;

    [SerializeField]
    private EventManager eventManager;

    [SerializeField]
    private MenuNavigator PauseMenu;

    [SerializeField]
    private GameObject HUD;

    private CursorLockMode previousCursorLockMode;

    private void Start()
    {
        InitUI();
    }

    private void OnEnable()
    {
        EventManager.OnOpenPauseMenu += OpenPauseMenu;
        EventManager.OnGameOver += OpenGameOverScreen;

        PauseMenu.OnMenuClosed += ClosePauseMenu;
    }

    private void OnDisable()
    {
        EventManager.OnOpenPauseMenu -= OpenPauseMenu;
        EventManager.OnGameOver -= OpenGameOverScreen;
        
        PauseMenu.OnMenuClosed -= ClosePauseMenu;
    }


    public void OpenPauseMenu()
    {
        previousCursorLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;

        PauseMenu.gameObject.SetActive(true);
        GameOverScreen.gameObject.SetActive(false);
        HUD.SetActive(false);
    }

    public void ClosePauseMenu()
    {
        Cursor.lockState = previousCursorLockMode;

        HUD.SetActive(true);
        PauseMenu.gameObject.SetActive(false);
        PauseMenu.ResetMenu();

        EventManager.OnPauseMenuClosedInvoke();
    }

    public void OpenGameOverScreen(string message)
    {
        Cursor.lockState = CursorLockMode.None;

        GameStateManager.PauseGame();
        
        GameOverScreen.gameObject.SetActive(true);
        HUD.SetActive(false);
        // set gameover message in ui
        GameOverScreen.GetComponentInChildren<GameOverMenu>().SetGameOverMessage(message);
    }

    private void InitUI()
    {
        HUD.SetActive(true);
        PauseMenu.gameObject.SetActive(false);
        GameOverScreen.gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralGameManager : MonoBehaviour
{
    private Health playerHealth;

    private GeneralGameManager Instance
    {
        get;
        set;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SubscribeToPlayerDeath;
        SceneManager.sceneLoaded += SetCommonOptions;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SubscribeToPlayerDeath;
        SceneManager.sceneLoaded -= SetCommonOptions;
    }


    // there can be multiple reasons for game over
    private void SubscribeToPlayerDeath(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= SendGameOverDeath;
            playerHealth = null;
        }

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null && playerController.TryGetComponent<Health>(out Health health))
        {
            playerHealth = health;
            playerHealth.OnDeath += SendGameOverDeath;
        }
    }

    private void SendGameOverDeath()
    {
        EventManager.SendGameOver("You died!");
    }

    private void SetCommonOptions(Scene scene, LoadSceneMode loadSceneMode)
    {
        Cursor.lockState = GameStateManager.CurrentState == GameState.MainMenu ? CursorLockMode.None : CursorLockMode.Locked;
        if (GameStateManager.CurrentState != GameState.MainMenu)
        {
            GameStateManager.UnpauseGame();
        }
    }






}

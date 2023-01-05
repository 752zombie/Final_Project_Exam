using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private string[] levelNames;

    [SerializeField]
    private int mainMenuBuildIndex;

    [SerializeField]
    private string[] commonScenesToInclude;

    public static LevelManager Instance
    {
        get;
        private set;
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
        
        if (SceneManager.GetActiveScene().buildIndex == mainMenuBuildIndex)
        {
            GameStateManager.CurrentState = GameState.MainMenu;
        }

        else
        {
            GameStateManager.CurrentState = GameState.Exploration;
        }

        LoadRestOfScene(null);
    }

    public string GetLevelName(int index)
    {
        if (levelNames.Length > index)
        {
            return levelNames[index];
        }

        return "Unknown";
    }

    public void LoadLevel(int index)
    {
        GameStateManager.CurrentState = index == mainMenuBuildIndex ? GameState.MainMenu : GameState.Exploration;
        SceneManager.LoadSceneAsync(index).completed += LoadRestOfScene;
    }

    public void LoadMainMenu()
    {
        LoadLevel(mainMenuBuildIndex);
    }


    private void LoadRestOfScene(AsyncOperation asyncOperation)
    {
        if (GameStateManager.CurrentState != GameState.MainMenu)
        {
            foreach (string scene in commonScenesToInclude)
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
        }
    }
}

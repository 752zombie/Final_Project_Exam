using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class PersistenceManager : MonoBehaviour
{
    public event Action OnSave;

    [SerializeField]
    private string gameDataFileExtension = ".sav";

    private GameData gameData;
    private int nextSaveId = 0;
    private string saveId = "-1";
    private FileHandler fileHandler;

    public static PersistenceManager Instance
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
        fileHandler = new FileHandler(Application.persistentDataPath);
        nextSaveId = PlayerPrefs.GetInt("nextSaveId", 1);
        gameData = new GameData();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // resets all GameData to default values
    public void NewGame()
    {
        Debug.Log("New game started");
        saveId = nextSaveId.ToString();
        gameData = new GameData();
    }

    private void UpdateNextId()
    {
        nextSaveId++;
        PlayerPrefs.SetInt("nextSaveId", nextSaveId);
    }

    private void LoadGame()
    {
        gameData = fileHandler.Load(saveId + gameDataFileExtension);

        if (gameData == null)
        {
            Debug.Log("No save found");
            NewGame();
        }

        List<ISaveable> saveableObjects = FindAllSaveableObjects();

        foreach (ISaveable saveable in saveableObjects)
        {
            saveable.Load(gameData);
        }
    }

    public void LoadGame(string saveId)
    {
        this.saveId = saveId;
        LoadGame();
    }

    private void SaveGame()
    {
        OnSave?.Invoke();

        List<ISaveable> saveableObjects = FindAllSaveableObjects();
        
        foreach (ISaveable saveable in saveableObjects)
        {
            saveable.Save(gameData);
        }

        gameData.LevelIndex = SceneManager.GetActiveScene().buildIndex;
        gameData.LastPlayed = DateTime.Now.ToBinary();

        fileHandler.Save(saveId + gameDataFileExtension, gameData);
    }

    public void SaveGame(string saveId)
    {
        this.saveId = saveId;
        SaveGame();
    }


    // creates a new save file and saves the current GameData in it
    public void SaveNew()
    {
        saveId = nextSaveId.ToString();
        UpdateNextId();
        SaveGame();
    }

    private List<ISaveable> FindAllSaveableObjects()
    {
        IEnumerable<ISaveable> objects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
        return new List<ISaveable>(objects);
    }

    public Dictionary<string, GameData> GetAllSaves()
    {
        return fileHandler.LoadAllSaves();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        LoadGame();
    }
}

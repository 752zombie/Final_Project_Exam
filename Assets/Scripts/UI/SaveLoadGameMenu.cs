using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System;

public class SaveLoadGameMenu : MonoBehaviour, INavigatable
{
    [SerializeField]
    private GameObject saveLoadButtonPrebab;

    [SerializeField]
    private GameObject contentContainer;

    [SerializeField]
    private Selectable NewSaveButton;

    public bool IsSaveMenu = false;

    private Selectable firstSelected;

    private void OnDisable()
    {
        Clean();
    }

    private void GenerateSaveButtons()
    {
        Dictionary<string, GameData> saveFiles = PersistenceManager.Instance.GetAllSaves();
        List<KeyValuePair<string, GameData>> keyValuePairs = saveFiles.ToList();
        keyValuePairs.Sort((x, y) => DateTime.Compare(DateTime.FromBinary(y.Value.LastPlayed), DateTime.FromBinary(x.Value.LastPlayed)));

        List<GameObject> gameObjects = new List<GameObject>();

        foreach (KeyValuePair<string, GameData> keyValuePair in keyValuePairs)
        {
            GameObject instantiatedGameObject = Instantiate(saveLoadButtonPrebab, contentContainer.transform);
            SaveLoadButton saveLoadButton = instantiatedGameObject.GetComponent<SaveLoadButton>();
            saveLoadButton.SetSaveId(keyValuePair.Key);
            saveLoadButton.SetLevelName(LevelManager.Instance.GetLevelName(keyValuePair.Value.LevelIndex));
            saveLoadButton.SetLevelIndex(keyValuePair.Value.LevelIndex);
            saveLoadButton.SetDateSaved(DateTime.FromBinary(keyValuePair.Value.LastPlayed).ToString());
            saveLoadButton.SetMenu(this);

            gameObjects.Add(instantiatedGameObject);
        }

        if (IsSaveMenu)
        {
            firstSelected = NewSaveButton;
        }

        else if (gameObjects.Count > 0)
        {
            firstSelected = gameObjects[0].GetComponentInChildren<Selectable>();
        }

        else
        {
            firstSelected = GetComponentInChildren<Selectable>();
        }
    }

    public void LoadGame(string saveId, int levelIndex)
    {
        PersistenceManager.Instance.LoadGame(saveId);
        LevelManager.Instance.LoadLevel(levelIndex);
    }

    public void NewSaveGame()
    {
        PersistenceManager.Instance.SaveNew();
        Clean();
        GenerateSaveButtons();
        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }

    public void SaveGame(string saveId)
    {
        PersistenceManager.Instance.SaveGame(saveId);
        Clean();
        GenerateSaveButtons();
        EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
    }


    private void Clean()
    {
        foreach (Transform child in contentContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public Selectable FirstSelected()
    {
        return firstSelected;
    }

    public void OnNavigatedTo()
    {
        GenerateSaveButtons();
    }
}

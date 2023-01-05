using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveLoadButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI saveName;

    [SerializeField]
    private TextMeshProUGUI dateSaved;

    [SerializeField]
    private TextMeshProUGUI levelName;

    private string saveId;

    private int levelIndex;

    private SaveLoadGameMenu saveLoadGameMenu;

    public void SetSaveId(string saveId)
    {
        this.saveId = saveId;
        saveName.text = "Save " + saveId;
    }

    public void SetLevelName(string name)
    {
        levelName.text = name;
    }

    public void SetLevelIndex(int index)
    {
        levelIndex = index;
    }

    public void SetDateSaved(string date)
    {
        dateSaved.text = date;
    }

    public void SetMenu(SaveLoadGameMenu saveLoadGameMenu)
    {
        this.saveLoadGameMenu = saveLoadGameMenu;
    }

    public void OnClick()
    {
        if (saveLoadGameMenu == null)
        {
            return;
        }

        if (saveLoadGameMenu.IsSaveMenu)
        {
            saveLoadGameMenu.SaveGame(saveId);
        }

        else
        {
            saveLoadGameMenu.LoadGame(saveId, levelIndex);
        }
    }
}

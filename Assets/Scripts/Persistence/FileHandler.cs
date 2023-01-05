using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileHandler
{
    private string dirPath;

    public FileHandler(string dirPath)
    {
        this.dirPath = dirPath;
    }

    public void Save(string fileName, GameData gameData)
    {
        string path = Path.Combine(dirPath, "saves", fileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string serializedData = JsonUtility.ToJson(gameData, true);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(serializedData);
                }
            }
        }

        catch (Exception e)
        {
            Debug.Log("Error: " + e);
        }
    }

    public GameData Load(string fileName)
    {
        string path = Path.Combine(dirPath, "saves", fileName);
        GameData loadedGameData = null;

        if (File.Exists(path))
        {
            try
            {
                string serializedData = "";
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        serializedData = streamReader.ReadToEnd();
                    }
                }

                loadedGameData = JsonUtility.FromJson<GameData>(serializedData);
            }

            catch (Exception e)
            {
                Debug.Log("Error: " + e);
            }
        }

        return loadedGameData;
    }

    public Dictionary<string, GameData> LoadAllSaves()
    {
        Dictionary<string, GameData> savesDict = new Dictionary<string, GameData>();

        string path = Path.Combine(dirPath, "saves");

        if (Directory.Exists(path))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            IEnumerable<FileInfo> fileInfos = directoryInfo.EnumerateFiles();

            foreach (FileInfo fileInfo in fileInfos)
            {
                GameData gameData = Load(fileInfo.Name);
                string[] splitName = fileInfo.Name.Split('.');
                savesDict.Add(splitName[0], gameData);
            }
        }

        return savesDict;
    }


    // not currently used, but could maybe be used in the future
    private string EncryptDecrypt(string data, string encryptionKey)
    {
        string dataToReturn = "";

        for (int i = 0; i < data.Length; i++)
        {
            dataToReturn += data[i] ^ encryptionKey[i % encryptionKey.Length];
        }

        return dataToReturn;
    }
}

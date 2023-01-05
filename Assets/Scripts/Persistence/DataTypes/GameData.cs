using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long LastPlayed;
    public PlayerData PlayerData;
    public SerializableDictionary<string, PositionAndRotation> MovableObjectPositionsAndRotations;
    public SerializableDictionary<string, int> Health;
    public int LevelIndex;

    public GameData()
    {
        PlayerData = new PlayerData();
        MovableObjectPositionsAndRotations = new SerializableDictionary<string, PositionAndRotation>();
        Health = new SerializableDictionary<string, int>();
    }
}

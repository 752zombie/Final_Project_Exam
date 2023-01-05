using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector3 Position;
    public Quaternion Rotation;

    public PlayerData(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public PlayerData()
    {
        Position = new Vector3(0, 0, 0);
        Rotation = Quaternion.Euler(0, 0, 0);
    }
}

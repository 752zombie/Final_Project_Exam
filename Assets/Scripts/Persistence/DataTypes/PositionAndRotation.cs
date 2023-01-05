using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionAndRotation
{
    public Vector3 Position;
    public Quaternion Rotation;

    public PositionAndRotation(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePositionAndRotation : MonoBehaviour, ISaveable
{
    [SerializeField]
    private string id;

    [ContextMenu("Generate unique id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void Load(GameData gameData)
    {
        if (gameData.MovableObjectPositionsAndRotations.TryGetValue(id, out PositionAndRotation positionAndRotation))
        {
            transform.SetPositionAndRotation(positionAndRotation.Position, positionAndRotation.Rotation);
        }
    }

    public void Save(GameData gameData)
    {
        gameData.MovableObjectPositionsAndRotations[id] = new PositionAndRotation(transform.position, transform.rotation);
    }

}

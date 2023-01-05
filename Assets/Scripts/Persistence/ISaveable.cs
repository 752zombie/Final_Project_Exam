using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public void Save(GameData gameData);

    public void Load(GameData gameData);
}

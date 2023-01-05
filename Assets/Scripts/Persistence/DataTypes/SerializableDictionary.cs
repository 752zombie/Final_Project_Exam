using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keyList = new List<TKey>();

    [SerializeField]
    private List<TValue> valueList = new List<TValue>();

    public void OnAfterDeserialize()
    {
        Clear();

        for (int i = 0; i < keyList.Count; i++)
        {
            Add(keyList[i], valueList[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        keyList.Clear();
        valueList.Clear();

        foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
        {
            keyList.Add(keyValuePair.Key);
            valueList.Add(keyValuePair.Value);
        }
    }
}

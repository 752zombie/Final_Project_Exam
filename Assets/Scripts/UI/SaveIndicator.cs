using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject saveIndicator;

    private PersistenceManager persistenceManager;

    private void Awake()
    {
        persistenceManager = PersistenceManager.Instance;

        if (persistenceManager != null)
        {
            persistenceManager.OnSave += OnSave;
        }
    }

    private void OnDestroy()
    {
        if (persistenceManager != null)
        {
            persistenceManager.OnSave -= OnSave;
        }
    }

    private void OnSave()
    {
        StartCoroutine(DisplaySaveIndicator());
    }

    private IEnumerator DisplaySaveIndicator()
    {
        saveIndicator.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        saveIndicator.SetActive(false);
    }
}

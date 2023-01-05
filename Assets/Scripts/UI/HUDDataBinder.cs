using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDDataBinder : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBar;

    [SerializeField]
    private InteractPrompt interactPrompt;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Bind;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Bind;
    }

    private void Bind(Scene scene, LoadSceneMode loadSceneMode)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null && playerController.TryGetComponent<Health>(out Health health))
        {
            healthBar.Bind(health);
        }

        if (playerController != null && playerController.TryGetComponent<Interact>(out Interact interact))
        {
            interactPrompt.Bind(interact);
        }


    }
}

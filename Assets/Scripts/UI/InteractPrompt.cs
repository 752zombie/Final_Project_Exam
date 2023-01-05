using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI interactObjectText;

    private Interact interact;

    public void Bind(Interact playerInteract)
    {
        if (interact != null)
        {
            interact.OnInteractbleChanged -= UpdatePrompt;
        }

        interact = playerInteract;

        interact.OnInteractbleChanged += UpdatePrompt;
    }

    private void UpdatePrompt(string objectText)
    {
        if (objectText == null || objectText == "")
        {
            gameObject.SetActive(false);
        }

        else
        {
            gameObject.SetActive(true);
            interactObjectText.text = objectText;
        }
    }

    private void OnDestroy()
    {
        if (interact != null)
        {
            interact.OnInteractbleChanged -= UpdatePrompt;
        }
    }


}

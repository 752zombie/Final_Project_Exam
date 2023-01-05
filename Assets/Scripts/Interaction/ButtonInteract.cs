using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class ButtonInteract : MonoBehaviour, Interactable
{
    [Tooltip("If true the button will toggle between being activated and deactivated with each press")]
    public bool IsToggle = false;

    private bool isActivated = false;
    private Animator animator;
    private InteractableObject interactableObject;
    private readonly string NameOfInteractable = "Button";

    private void Awake()
    {
        interactableObject = GetComponent<InteractableObject>();
        animator = GetComponent<Animator>();
    }
    public void Interact()
    {
        if (IsToggle)
        {
            isActivated = !isActivated;
            interactableObject.SetIsActivated(isActivated);

            if (animator != null)
            {
                animator.SetBool("Press", isActivated);
            }
        }

        else
        {
            if (animator != null)
            {
                StartCoroutine(ActivateDeactivateAnimation());
            }

            interactableObject.SetIsActivated(true);
        }

        interactableObject.SubmitInteraction();
    }

    private IEnumerator ActivateDeactivateAnimation()
    {
        animator.SetBool("Press", true);
        yield return new WaitForEndOfFrame();
        animator.SetBool("Press", false);
    }

    public string GetNameOfInteractable()
    {
        return NameOfInteractable;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class PressurePlateInteract : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Pressure plate will only trigger on these layers.")]
    private LayerMask TriggerLayers;

    private Animator animator;
    private InteractableObject interactableObject;

    private int amountInTrigger = 0;

    private void Awake()
    {
        interactableObject = GetComponent<InteractableObject>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ShouldTrigger(other.gameObject) && ++amountInTrigger == 1)
        {
            interactableObject.SetIsActivated(true);
            interactableObject.SubmitInteraction();
            animator.SetBool("IsTriggered", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ShouldTrigger(other.gameObject) && --amountInTrigger == 0)
        {
            interactableObject.SetIsActivated(false);
            interactableObject.SubmitInteraction();
            animator.SetBool("IsTriggered", false);
        }
    }

    private bool ShouldTrigger(GameObject gameObject)
    {
        return (TriggerLayers & (1 << gameObject.layer)) != 0;
    }
}

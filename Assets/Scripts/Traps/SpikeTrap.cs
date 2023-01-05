using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Trap
{
    [SerializeField]
    private InteractableObject interactableObject;

    public bool DefaultActivated = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (interactableObject != null)
        {
            interactableObject.OnInteract += OnInteract;
        }
    }

    private void Start()
    {
        animator.SetBool("Activate", DefaultActivated);
    }

    private void OnInteract(InteractableObject interactableObject)
    {
        if (interactableObject.IsActivated)
        {
            Activate();
        }

        else
        {
            Deactivate();
        }
    }

    public override void Activate()
    {
        animator.SetBool("Activate", true);
    }

    public override void Deactivate()
    {
        animator.SetBool("Activate", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private InteractableObject interactableObject;

    private Animator animator;
    private bool isOpen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        interactableObject.OnInteract += OpenCloseDoor;
    }

    private void OpenCloseDoor(InteractableObject interactableObject)
    {
        isOpen = interactableObject.IsActivated;
        animator.SetBool("Open", isOpen);
    }
}

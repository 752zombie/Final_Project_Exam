using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// base class of all objects that can be interacted with 
public class InteractableObject : MonoBehaviour
{
    public event Action<InteractableObject> OnInteract;

    private float progress = 1;
    private bool isActivated = true;

    public float InteractionProgress
    {
        get => progress;
    }

    public bool IsActivated
    {
        get => isActivated;
    }

    public void SubmitInteraction()
    {
        OnInteract?.Invoke(this);
    }

    public void SetProgress(float progress)
    {
        this.progress = progress;
    }

    public void SetIsActivated(bool isActivated)
    {
        this.isActivated = isActivated;
    }
}

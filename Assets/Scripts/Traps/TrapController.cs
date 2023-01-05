using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapControllerActivation
{
    All,
    Sequential
}

public class TrapController : MonoBehaviour
{
    [SerializeField]
    private TrapControllerActivation activationMode;

    [SerializeField]
    private InteractableObject interactableObject;

    [SerializeField]
    private List<Trap> traps;

    [SerializeField]
    private float SequenceDelay;

    [SerializeField]
    private bool InvertActivation = false;

    private void Awake()
    {
        interactableObject.OnInteract += Activate;
    }

    private void Activate(InteractableObject interactableObject)
    {
        bool activationSignal = InvertActivation ? !interactableObject.IsActivated : interactableObject.IsActivated;

        if (activationMode == TrapControllerActivation.All)
        {
            ActivateAll(activationSignal);
        }
        
        else
        {
            StartCoroutine(ActivateSequence(activationSignal));
        }
    }


    private void ActivateAll(bool shouldActivate)
    {
        foreach (Trap trap in traps)
        {
            if (shouldActivate)
            {
                trap.Activate();
            }

            else
            {
                trap.Deactivate();
            }
        }
    }

    private IEnumerator ActivateSequence(bool shouldActivate)
    {
        for (int i = 0; i < traps.Count; i++)
        {
            if (shouldActivate)
            {
                traps[i].Activate();
            }

            else
            {
                traps[i].Deactivate();
            }

            yield return new WaitForSeconds(SequenceDelay);
        }
    }
}

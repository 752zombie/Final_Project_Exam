using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interact : MonoBehaviour
{
    public event Action<string> OnInteractbleChanged;

    [Tooltip("The layers that should be considered interactable objects")]
    [SerializeField]
    private LayerMask layerMask;

    [Tooltip("The maximum distance that the player should be able to interact with InteractableObject's from")]
    public float InteractDistance = 1;

    // we will cast a ray relative to the camera angle, not the player itself
    [SerializeField]
    private Transform cinemachineFollowTarget;
    
    private GameObject chosenInteractableGameObject;
    private Interactable inputInteractable;

    void Update()
    {
        FindInteractable();

        if (Input.GetKeyDown(KeyCode.E) && inputInteractable != null)
        {
            inputInteractable.Interact();
        }
    }

    private void FindInteractable()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(cinemachineFollowTarget.position, cinemachineFollowTarget.forward, InteractDistance, layerMask, QueryTriggerInteraction.Collide);

        Vector3 rayEndPosition = cinemachineFollowTarget.position + cinemachineFollowTarget.forward * InteractDistance;

        GameObject lastInteractable = chosenInteractableGameObject;

        // reset chosenInteractable
        if (hits.Length == 0)
        {
            chosenInteractableGameObject = null;
            inputInteractable = null;
        }

        // if multiple available interactables, determine which one to interact with based on distance from objects where ray ends
        foreach (RaycastHit hit in hits)
        {
            if (chosenInteractableGameObject == null)
            {
                chosenInteractableGameObject = hit.transform.gameObject;
            }

            else
            {
                float chosenInteractableMagnitude = (chosenInteractableGameObject.transform.position - rayEndPosition).sqrMagnitude;
                float hitMagnitude = (hit.transform.position - rayEndPosition).sqrMagnitude;

                chosenInteractableGameObject = hitMagnitude < chosenInteractableMagnitude ? hit.transform.gameObject : chosenInteractableGameObject;
            }

        }

        // Interactable changed
        // Only call GetComponent if neccesary as it can be expensive
        if (!ReferenceEquals(lastInteractable, chosenInteractableGameObject)) {
            
            if (chosenInteractableGameObject != null)
            {
                inputInteractable = chosenInteractableGameObject.GetComponent<Interactable>();
            }

            // display button prompt
            if (inputInteractable != null)
            {
                OnInteractbleChanged?.Invoke(inputInteractable.GetNameOfInteractable());
            }

            else
            {
                OnInteractbleChanged?.Invoke(null);
            }
        }

        // To visualise the ray in scene view
        Debug.DrawRay(cinemachineFollowTarget.position, cinemachineFollowTarget.forward.normalized * InteractDistance, Color.green);
    }
}

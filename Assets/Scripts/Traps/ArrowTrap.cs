using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : Trap
{
    [SerializeField]
    private InteractableObject[] interactableObjects;

    [Tooltip("How many times per second should the trap fire when activated.")]
    public float FireRate = 0.5f;

    public float projectileVelocity = 5;

    [SerializeField]
    private bool FireWithoutTrigger = false;

    [SerializeField]
    private bool ConstantFireWhileActivated = false;

    [SerializeField]
    private GameObject projectile;
   
    private Coroutine activeRoutine;

    private void Awake()
    {
        foreach (InteractableObject interactableObject in interactableObjects)
        {
            interactableObject.OnInteract += OnInteract;
        }

        // To prevent negative fire rate and division by zero.
        FireRate = FireRate <= 0 ? 0.1f : FireRate;
    }

    private void Start()
    {
        if (FireWithoutTrigger)
        {
            StartFiring();
        }
    }

    private void OnInteract(InteractableObject interactableObject)
    {
        if (interactableObject.IsActivated)
        {
            StartFiring();
        }

        else
        {
            StopFiring();
        }
    }

    private void StartFiring()
    {
        if (ConstantFireWhileActivated)
        {
            if (activeRoutine != null)
            {
                StopCoroutine(activeRoutine);
            }
            
            activeRoutine = StartCoroutine(FireRepeating());
        }

        else
        {
            FireProjectile();
        }
    }

    private void StopFiring()
    {
        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
        }
    }

    private void FireProjectile()
    {
        GameObject gameObject = Instantiate(projectile, transform.position, transform.rotation);
        gameObject.GetComponent<Rigidbody>().AddForce(transform.up * projectileVelocity, ForceMode.VelocityChange);
    }

    private IEnumerator FireRepeating()
    {
        while(true)
        {
            FireProjectile();
            yield return new WaitForSeconds(1 / FireRate);
        }
    }

    public override void Activate()
    {
        StartFiring();
    }

    public override void Deactivate()
    {
        StopFiring();
    }
}

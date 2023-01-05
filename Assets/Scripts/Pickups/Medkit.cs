using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    [SerializeField]
    private int healAmount;

    [SerializeField]
    private AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            // Don't waste the medkit if the player isn't injured
            if (health.CurrentHealth < health.MaxHealth)
            {
                health.Heal(healAmount);

                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }
                
                Destroy(gameObject);
            }
        }
    }
}

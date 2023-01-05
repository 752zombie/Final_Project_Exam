using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpikeTriggerAction
{
    Kill,
    Damage
}

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private SpikeTriggerAction spikeTriggerAction;

    public int Damage = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            if (spikeTriggerAction == SpikeTriggerAction.Kill)
            {
                health.Kill();
            }

            else
            {
                health.TakeDamage(Damage);
            }
        }
    }
}

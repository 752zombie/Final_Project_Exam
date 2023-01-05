using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField]
    private float CollisonDelay = 0.1f;

    private float timeSinceSpawn = 0;

    void Update()
    {
        timeSinceSpawn += Time.deltaTime;
        if (timeSinceSpawn > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.TryGetComponent(out Health health) && other.CompareTag("Player"))
        {
            health.TakeDamage(20);
            Destroy(gameObject);
        }

        else if (timeSinceSpawn > CollisonDelay && !other.CompareTag("Dart"))
        {
            Destroy(gameObject);
        }
    }
}

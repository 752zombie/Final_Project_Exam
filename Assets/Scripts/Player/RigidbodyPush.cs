using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyPush : MonoBehaviour
{
    [SerializeField]
    private LayerMask PushableLayers;
    public float PushStrength = 0.7f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        int hitLayer = hit.gameObject.layer;

        if ((PushableLayers & (1 << hitLayer)) == 0)
        {
            return;
        }

        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null)
        {
            rigidbody.AddForce(new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z).normalized * PushStrength, ForceMode.VelocityChange);
        }
    }
}

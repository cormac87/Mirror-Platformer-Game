using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    // Layer name to check for
    public string targetLayer = "Mirror";

    public FlipObject flipScript;

    // Cooldown time in seconds
    public float cooldownTime = 3f;

    // Last time a collision was processed
    private float lastCollisionTime = -Mathf.Infinity;

    void OnTriggerEnter(Collider other)
    {
        // Check if the cooldown period has passed
        if (Time.time >= lastCollisionTime + cooldownTime)
        {
            // Check if the object is on the specified layer
            if (other.gameObject.layer == LayerMask.NameToLayer(targetLayer))
            {
                // Update the last collision time to the current time
                lastCollisionTime = Time.time;

                Debug.Log("Called");

                // Call the flipCharacter method on the flipScript
                flipScript.flipCharacter(other.gameObject);
            }
        }
    }
}

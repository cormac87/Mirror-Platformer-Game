using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipObject : MonoBehaviour
{
    private GameObject envFlipParent;

    public GameObject parent;

    public GameObject mirrorPrefab;

    private Vector3 planeNormal;

    private Rigidbody rb;

    private bool holding_mirror = false;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void setHoldingMirror(bool holding)
    {
        holding_mirror = holding;
    }

    public void flipCharacter(GameObject mirror)
    {
        if (!holding_mirror)
        {
            // Create a new GameObject to act as the flipped parent
            envFlipParent = new GameObject();
            envFlipParent.transform.position = mirror.transform.position;
            envFlipParent.transform.rotation = mirror.transform.rotation;

            // Move all children of the parent to the new flipped parent using a for loop
            int childCount = parent.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = parent.transform.GetChild(i);
                child.SetParent(envFlipParent.transform, true);
            }

            // Flip the environment by scaling the new parent
            envFlipParent.transform.localScale = new Vector3(1, 1, -1);

            List<GameObject> mirrorsToDestroy = new List<GameObject>();

            // Use a for loop to process each child in the flipped parent
            for (int i = envFlipParent.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = envFlipParent.transform.GetChild(i);

                if (LayerMask.LayerToName(child.gameObject.layer) == "Mirror")
                {
                    // Rotate the child to correct the orientation after flipping
                    child.Rotate(0, 180, 0, Space.Self);
                    Vector3 up = child.up;
                    Vector3 forward = child.forward;
                    Vector3 mirrorScale = child.localScale;

                    // Instantiate a new mirror and set its position and rotation
                    GameObject newMirror = Instantiate(mirrorPrefab);
                    newMirror.transform.position = child.position;
                    Quaternion rotation = Quaternion.LookRotation(forward, up);
                    newMirror.transform.rotation = rotation;
                    newMirror.transform.localScale = new Vector3(mirrorScale.x, mirrorScale.y, 1);
                    // Add the old mirror to the list for destruction
                    mirrorsToDestroy.Add(child.gameObject);

                    // Set the new mirror's parent back to the original parent
                    newMirror.transform.SetParent(parent.transform, true);
                }
                else
                {
                    // Re-parent non-mirror children back to the original parent
                    child.SetParent(parent.transform, true);
                }
            }

            // Destroy the old mirrors after re-parenting
            while (mirrorsToDestroy.Count > 0)
            {
                mirrorsToDestroy[0].SetActive(false);
                mirrorsToDestroy.RemoveAt(0);
            }
        }
    }
}

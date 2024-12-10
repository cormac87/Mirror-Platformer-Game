using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{

    public Transform character;
    void Update()
    {
        transform.position = character.position + character.up * 0.5f;
    }
}

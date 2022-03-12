using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clamp mask to ensure its rotation and position are consistent
public class Mask_Clamp : MonoBehaviour
{
    private Rigidbody2D rb;
    private float rotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rotation = 0f;
    }

    void Update()
    {
        transform.position = transform.parent.position;
    }
}

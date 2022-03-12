using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player can move left and right
public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 move_dir;
    private float move_speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move_dir = Vector2.zero;
        move_speed = 2f;
    }

    void Update()
    {
        move_dir.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.AddForce(move_dir.normalized * move_speed, ForceMode2D.Impulse);
    }
}

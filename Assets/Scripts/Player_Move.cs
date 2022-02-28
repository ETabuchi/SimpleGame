using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles a simple movement script for player
public class Player_Move : MonoBehaviour
{
    // GameObject Components
    private Rigidbody2D rb;

    // Movement direction represented by vector
    private Vector2 move_dir;
    private Vector2 jump_dir;

    // Movement speeds
    private float move_speed;
    public float jump_speed;
    
    // Checks if you can jump (only when touching ground)
    public bool touch_ground;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move_dir = Vector2.zero;
        jump_dir = Vector2.up;
        move_speed = 1.5f;
        jump_speed = 50f;
    }

    void Update()
    {
        move_dir.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && touch_ground)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(jump_dir * jump_speed, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        rb.AddForce(move_dir.normalized * move_speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            touch_ground = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            touch_ground = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player can only move left and right, so up and down do nothing
public class Player_Paddle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 move_dir;
    private float move_speed; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
        move_dir = Vector2.zero;
        move_speed = 10f;
    }

    void Update()
    {
        move_dir.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.velocity = move_dir.normalized * move_speed * Speed_Manager.game_speed;
    }
}

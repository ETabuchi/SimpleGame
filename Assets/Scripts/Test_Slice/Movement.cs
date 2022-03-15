using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player can move left and right
public class Movement : MonoBehaviour
{
    // Movement
    private Rigidbody2D rb;
    private Vector2 move_dir;
    private float move_speed;
    private float jump_force;
    public static bool touching_ground;
    public static bool can_move;

    // Visuals / Audio
    private SpriteRenderer sprite;
    private Animator anim;

    void Start()
    {
        // Movement
        rb = GetComponent<Rigidbody2D>();
        move_dir = Vector2.zero;
        move_speed = 1.3f;
        jump_force = 50f;
        touching_ground = true;
        can_move = true;

        // Visuals / Audio
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D)) // Move right
        {
            if (!Input.GetKey(KeyCode.A))
            {
                sprite.flipX = false;
            }
        }
        else if (Input.GetKey(KeyCode.A)) // Move left
        {
            sprite.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck()) // Jump
        {
            rb.AddForce(jump_force * Vector2.up, ForceMode2D.Impulse);
        }

        move_dir.x = Input.GetAxisRaw("Horizontal"); // Horizontal movement
        GroundCheck();
        MoveAnim();
    }

    private void FixedUpdate()
    {
        if (can_move)
        {
            rb.AddForce(move_dir.normalized * move_speed, ForceMode2D.Impulse);
        }
    }

    // Check if player is touching the ground to jump
    bool GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            touching_ground = true;
            anim.SetBool("jumping", false);
            return true;
        }

        touching_ground = false;
        anim.SetBool("jumping", true);
        return false;
    }

    // Movement Animation Check
    void MoveAnim()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("walking", false);
        }
        else
        {
            anim.SetBool("walking", true);
        }
    }
}

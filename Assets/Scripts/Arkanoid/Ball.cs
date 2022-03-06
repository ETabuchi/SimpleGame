using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Script for projectile fired by player
// Also keeps track of number of remaining blocks to determine if the player has won yet
public class Ball : MonoBehaviour
{
    private Rigidbody2D ball_rb;  // For ball's speed
    private Transform player_pos; // Where player is
    private Vector3 offset;       // Ball will spawn above player
    private Vector2 move_dir;     // Where the ball will go when fired
    private float move_speed;
    private float max_speed;
    public static bool is_active; // Ball can only be fired when inactive (at start of round / new round)
    public static int num_remain; // How many blocks remain
    private AudioSource block_hit; // Sound that plays when a block gets destroyed
    private AudioSource richochet; // Sound that plays when ball hits anything else

    void Start()
    {
        // GameObject Components
        ball_rb = GetComponent<Rigidbody2D>();
        player_pos = GameObject.Find("Player").transform;

        // Stats related to ball movement
        offset = new Vector3(0, 1, 0);
        move_dir = Vector2.up;
        move_speed = 0f;
        max_speed = 10f;
        is_active = false;

        // Remaining blocks / Audio
        num_remain = GameObject.FindGameObjectsWithTag("Block").Length;
        block_hit = GetComponent<AudioSource>();
        richochet = transform.GetChild(0).GetComponent<AudioSource>();
    }

    void Update()
    {
        // Press LMB to fire ball
        if (Input.GetMouseButtonDown(0) && !is_active && Camera.main.ScreenToWorldPoint(Input.mousePosition).y > player_pos.position.y)
        {
            is_active = true;
        }

        // Can only aim while ball is not active
        if (!is_active)
        {
            transform.position = player_pos.position + offset;
            move_dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player_pos.position;
            move_speed = 0f;
        }
        else
        {
            // Else ball is active and that influences movement
            move_speed = max_speed;
        }

        // Victory condition: num_remain <= 0
        if (num_remain <= 0)
        {
            Player_UI.victory.gameObject.SetActive(true); // Enable victory screen

            Destroy(gameObject);                           // Destroy ball
            Destroy(GameObject.Find("Player"));            // Destroy player
        }
    }

    // Move the ball
    private void FixedUpdate()
    {
        ball_rb.velocity = move_dir.normalized * move_speed * Speed_Manager.game_speed;
    }

    // Ball will be reflected off of surfaces it hits
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            num_remain--;
            block_hit.Play();
            Destroy(collision.gameObject);
        }
        else
        {
            richochet.Play();
        }

        if (is_active)
        {
            move_dir = Vector2.Reflect(move_dir, collision.contacts[0].normal);
        }
    }

    // Reset onto player if you fail to touch ball
    private void OnBecameInvisible()
    {
        is_active = false;

        if (num_remain > 0)
        {
            if (Player_Lives.num_lives <= 1)
            {
                Player_Lives.num_lives--;
                if (Player_UI.life_count != null)
                {
                    Player_UI.life_count.text = Player_Lives.num_lives.ToString();
                }
                Player_UI.gameover.gameObject.SetActive(true); // Enable game over screen

                Destroy(gameObject);                           // Destroy ball
                Destroy(GameObject.Find("Player"));            // Destroy player
            }
            else
            {
                Player_Lives.num_lives--;
                if (Player_UI.life_count != null)
                {
                    Player_UI.life_count.text = Player_Lives.num_lives.ToString();
                }

                if (GetComponent<Transform>() != null && player_pos != null)
                {
                    transform.position = player_pos.position + offset;
                }
            }
        }
    }
}

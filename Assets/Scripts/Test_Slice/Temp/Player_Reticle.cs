using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player reticle moves where your mouse is and rotates around player
public class Player_Reticle : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player_pos;
    private float rot_angle;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player_pos = GameObject.Find("Player").transform;
        rot_angle = 0f;
    }

    void Update()
    {
        // Move reticle where mouse / cursor is
        transform.position = Vector2.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 10f);

        // Rotate reticle around player
        rot_angle = Mathf.Atan2(transform.position.y - player_pos.position.y, transform.position.x - player_pos.position.x) * Mathf.Rad2Deg + 270f;
        rb.rotation = rot_angle;

    }
}

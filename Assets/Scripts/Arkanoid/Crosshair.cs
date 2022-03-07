using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The crosshair will be visible when you have to fire a shot
// Otherwise, it turns invisible for the player to have more vision 
public class Crosshair : MonoBehaviour
{
    private SpriteRenderer crosshair; // sprite is disabled when ball is in motion

    private void Start()
    {
        Cursor.visible = false;
        crosshair = GetComponent<SpriteRenderer>();
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 10f);

        if (!Ball.is_active || Speed_Manager.game_paused || Speed_Manager.won)
        {
            crosshair.enabled = true;
        }
        else
        {
            crosshair.enabled = false;
        }
    }
}

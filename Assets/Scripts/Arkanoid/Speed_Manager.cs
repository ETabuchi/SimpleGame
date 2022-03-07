using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls how fast the game will run
// When the ball is moving along, press fast forward to speed things up
// When you want a break, press pause to stop the action
public class Speed_Manager : MonoBehaviour
{
    public static float game_speed;   // How fast the game is currently running
    public static float fast_forward; // How fast the game will be fast-forwarded
    public static bool game_paused;   // If the game is paused or not
    public static bool speed_boost;   // If the game is fast-forwarded or not
    public static bool won;
    
    void Start()
    {
        game_speed = 1f;
        fast_forward = 1f; // Will increase upon pressing fast forward
        game_paused = false;
        won = false;
    }

    void Update()
    {
        if (Player_Lives.num_lives <= 0 || won)
        {
            Player_UI.pause.gameObject.SetActive(false);
        }

        // P or Escape - Pause Game
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) && Player_Lives.num_lives > 0 && !won)
        {
           if (game_paused)
           {
                // Unpause
                game_speed = fast_forward;
                game_paused = false;
                Player_UI.pause.gameObject.SetActive(false);
            }
           else
           {
                // Pause
                game_speed = 0f;
                game_paused = true;
                Player_UI.pause.gameObject.SetActive(true);
           }
        }

        // Shift - Fast Forward
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && Player_Lives.num_lives > 0 && !won)
        {
            if (speed_boost)
            {
                // Reduce speed
                fast_forward = 1f;
                game_speed = fast_forward;
                speed_boost = false;
                Player_UI.fast_forward.gameObject.SetActive(false);
            }
            else
            {
                // Increase speed
                fast_forward = 2.5f;
                game_speed = fast_forward;
                speed_boost = true;
                Player_UI.fast_forward.gameObject.SetActive(true);
            }
        }
    }
}

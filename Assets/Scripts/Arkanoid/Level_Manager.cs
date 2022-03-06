using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Contains functions that exist to manipulate level (i.e. restarting or quitting)

public class Level_Manager : MonoBehaviour
{
    // Take you to the test level
    public void PlayFirstLevel()
    {
        Player_Lives.num_lives = 3;
        Speed_Manager.game_speed = 1f;
        Speed_Manager.fast_forward = 1f;
        Speed_Manager.game_paused = false;
        Speed_Manager.speed_boost = false;
        SceneManager.LoadScene("Arkanoid_First_Level");
    }

    // Restart current level you are on
    public void Restart()
    {
        Player_Lives.num_lives = 3;
        Player_UI.fast_forward.gameObject.SetActive(false);
        Speed_Manager.game_speed = 1f;
        Speed_Manager.fast_forward = 1f;
        Speed_Manager.game_paused = false;
        Speed_Manager.speed_boost = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

    // Quit and return to main menu
    public void QuitLevel()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("Title_Screen");
    }

    // Quit to desktop
    public void QuitGame()
    {
        Application.Quit();
    }
}

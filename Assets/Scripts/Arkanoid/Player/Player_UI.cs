using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Creates a bunch of static UI elements that can be accessed by other scripts
public class Player_UI : MonoBehaviour
{
    public static Text life_count;
    public static Text gameover;
    public static Text pause;
    public static Image fast_forward;
    public static Text victory;
    void Start()
    {
        life_count = transform.GetChild(1).GetComponent<Text>();
        life_count.text = Player_Lives.num_lives.ToString();
        gameover = transform.GetChild(2).GetComponent<Text>();
        pause = transform.GetChild(3).GetComponent<Text>();
        fast_forward = transform.GetChild(4).GetComponent<Image>();
        victory = transform.GetChild(6).GetComponent<Text>();

        fast_forward.gameObject.SetActive(Speed_Manager.speed_boost);
    }
}

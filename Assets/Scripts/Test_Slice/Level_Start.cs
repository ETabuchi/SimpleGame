using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Level start up with camera close-up to player
// Camera zooms out once you have drawn out your weapon
public class Level_Start : MonoBehaviour
{
    // Animator attached to Cinemachine camera
    private Animator cam_anim;

    public static bool startup_finished;

    void Start()
    {
        cam_anim = GetComponent<Animator>();
        startup_finished = false;
    }

    void Update()
    {
        if (startup_finished)
        {
            cam_anim.Play("Gameplay");
        }
    }
}

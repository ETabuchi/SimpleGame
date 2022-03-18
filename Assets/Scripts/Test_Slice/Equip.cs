using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Toggle between equipped and un-equipped status
public class Equip : MonoBehaviour
{
    private Animator anim;
    private Image startup_button;
    private bool startup_done;
    public static bool equipped;

    void Start()
    {
        anim = GetComponent<Animator>();
        startup_button = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        startup_done = false;
        equipped = false; // start out without your weapon drawn

        anim.Play("Startup");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && startup_done)
        {
            Deactivate_UI();
            startup_done = false;
            anim.Play("Startup_Activate");
            Level_Start.startup_finished = true;
        }

        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle" && Level_Start.startup_finished)
        {
            Level_Start.startup_finished = false;
            StartCoroutine(SetAttack());
        }
    }

    // Fade in "activation button" to turn on your weapon
    IEnumerator StartUp_UI()
    {
        startup_done = true;
        Color temp_button = startup_button.color;
        for (float t = 0; t < 1.0f; t += 0.05f)
        {
            temp_button.a = t;
            startup_button.color = temp_button;
            if (!startup_done)
            {
                Deactivate_UI();
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Disable UI button input when pressing LMB
    private void Deactivate_UI()
    {
        Color transparent = startup_button.color;
        transparent.a = 0f;
        startup_button.color = transparent;
    }

    // Wait until player can attack / move
    private IEnumerator SetAttack()
    {
        yield return new WaitForSeconds(0.25f);
        Attack.is_attacking = false;
        Movement.can_move = true;
    }
}

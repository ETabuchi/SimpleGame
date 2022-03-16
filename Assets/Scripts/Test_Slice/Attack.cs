using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Standard melee attack with 3-hit combos, aerial attacks,
// charge attacks, recharging
public class Attack : MonoBehaviour
{
    // Counting which part of the combo you are at
    private int low_combo_index;
    private int low_combo_length;
    //private int high_combo_index;
    //private int high_combo_length;

    public static bool combo_mode; // false - low, true - high
    public static bool is_attacking;
    public static bool can_attack;
    private float attack_timer;
    private float attack_delay; // delay once combo is finished
    private float combo_timer; // counts upwards as soon as you melee once
    private float combo_delay; // delay between combos 
    //private float attack_time;
    private Animator anim;

    void Awake()
    {
        low_combo_index = 0;
        low_combo_length = 3;
        //high_combo_index = 0;
        //high_combo_length = 3;
        combo_mode = false; 
        is_attacking = false;
        can_attack = true;

        attack_timer = 0f;
        attack_delay = 1f;
        combo_timer = 0f;
        combo_delay = 0.5f;
        //attack_time = 0;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // combo_mode = !combo_mode;
        }

        if (Input.GetMouseButtonDown(0) && can_attack) // Melee Attack 
        {
            if (Movement.touching_ground)
            {
                is_attacking = true;

                // Ground combo
                if (combo_mode) // High attacks
                {

                }
                else // Low attacks
                {
                    switch (low_combo_index)
                    {
                        case 0:
                            anim.SetTrigger("low_combo_one");
                            combo_timer = 0f;
                            break;
                        case 1:
                            anim.ResetTrigger("low_combo_one");
                            anim.SetTrigger("low_combo_two");
                            anim.ResetTrigger("low_combo_three");
                            combo_timer = 0f;
                            break;
                        case 2:
                            anim.ResetTrigger("low_combo_one");
                            anim.ResetTrigger("low_combo_two");
                            anim.SetTrigger("low_combo_three");
                            combo_timer = 0f;
                            break;
                        default:
                            break;
                    }

                    low_combo_index = (low_combo_index + 1) % low_combo_length;
                    if (low_combo_index == 0)
                    {
                        // Add slight delay to combo
                        attack_timer = 0f;
                        is_attacking = false;
                        can_attack = false;
                    }
                }
            }
            else
            {
                // Aerial attack
                //is_attacking = true;
            }
        }

        // During mid-combo, increase timer to stop your combo eventually
        if (is_attacking && !Input.GetMouseButton(0))
        {
            combo_timer += Time.deltaTime;
        }

        if (combo_timer >= combo_delay)
        {
            is_attacking = false;
            combo_timer = 0f;
            low_combo_index = 0;
        }

        if (!can_attack)
        {
            attack_timer += Time.deltaTime;
        }

        if (attack_timer >= attack_delay)
        {
            attack_timer = 0f;
            can_attack = true;
        }
    }

    void ResetLowCombo()
    {
        anim.ResetTrigger("low_combo_one");
        anim.ResetTrigger("low_combo_two");
        anim.ResetTrigger("low_combo_three");
    }
}

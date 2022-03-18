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

    //private float attack_time;
    private Animator anim;

    void Awake()
    {
        low_combo_index = 0;
        low_combo_length = 3;
        //high_combo_index = 0;
        //high_combo_length = 3;
        combo_mode = false; 
        is_attacking = true; // will be set to false once weapon is drawn

        anim = GetComponent<Animator>();
    }

    void LowAttack()
    {
        is_attacking = false;
        if (low_combo_index < low_combo_length)
        {
            low_combo_index = (low_combo_index + 1) % low_combo_length;
        }
    }

    void LowComboEnd()
    {
        StartCoroutine(DelayNextAttack());
        low_combo_index = 0;
        Movement.can_move = true;
        Movement.can_jump = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // combo_mode = !combo_mode;
        }

        if (Input.GetMouseButtonDown(0) && !is_attacking && Movement.touching_ground) // Melee Attack 
        {
            is_attacking = true;
            anim.SetTrigger("low_" + low_combo_index);
            Movement.can_move = false;
            Movement.can_jump = false;
        }
    }

    // Delay next attack by slight amount of time to ensure animations look better
    IEnumerator DelayNextAttack()
    {
        yield return new WaitForSeconds(0.1f);
        is_attacking = false;
    }
}
